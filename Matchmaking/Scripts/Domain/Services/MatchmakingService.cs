using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;

namespace Xenocode.Features.Matchmaking.Scripts.Domain.Services
{
    public class MatchmakingService
    {
        private const int MAX_PLAYERS = 2;
        private const int HEARTBEAT_INTERVAL_MS = 15000;
        
        private bool _isJoining;
        private Lobby _currentLobby;
        private CancellationTokenSource _heartbeatCts;
        private string _gameSceneName = "BattleField";
        
        public event Action OnSessionFilled;
        public event Action<string> OnServerNotify;
        public event Action<string> OnServerMessage;

        public MatchmakingService()
        {
            if (NetworkManager.Singleton)
            {
                NetworkManager.Singleton.OnClientStopped += OnNetworkStopped;
                NetworkManager.Singleton.OnServerStopped += OnNetworkStopped;
            }
        }
        
        public async UniTask CreateOrJoinLobbyAsync(CancellationToken cancellationToken)
        {
            if (_isJoining) return;
            _isJoining = true;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                var joinedLobby = await TryQuickJoinLobbyAsync(cancellationToken);

                if (joinedLobby != null)
                {
                    _currentLobby = joinedLobby;
                    await JoinRelayAsClientAsync(_currentLobby);
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    _currentLobby = await CreateLobbyAsync(cancellationToken);
                    StartHostHeartbeat(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                await ShutdownAndCleanupAsync();
                throw; 
            }
            catch (Exception e)
            {
                await ShutdownAndCleanupAsync();
            }
            finally
            {
                _isJoining = false;
            }
        }

        private async UniTask<Lobby> TryQuickJoinLobbyAsync(CancellationToken cancellationToken)
        {
            try
            {
                var quickJoinOptions = new QuickJoinLobbyOptions
                {
                    Player = GetLocalPlayer()
                };
                
                return await LobbyService.Instance.QuickJoinLobbyAsync(quickJoinOptions);
            }
            catch (LobbyServiceException e)
            {
                if (e.Reason == LobbyExceptionReason.NoOpenLobbies || e.Reason == LobbyExceptionReason.RateLimited)
                {
                    OnServerNotify?.Invoke("No open lobbies found. Creating a new one...");
                    return null;
                }
                
                throw;
            }
        }
        

        private async UniTask<Lobby> CreateLobbyAsync(CancellationToken cancellationToken)
        {
            try
            {
                var allocation = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYERS);
                cancellationToken.ThrowIfCancellationRequested(); 

                var relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                cancellationToken.ThrowIfCancellationRequested();

                var options = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = GetLocalPlayer(),
                    Data = new Dictionary<string, DataObject>
                    {
                        { LobbyDataKeys.RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                    }
                };

                var lobbyName = $"GameLobby_{Guid.NewGuid().ToString().Substring(0, 5)}";
                
                var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, MAX_PLAYERS, options);                
                OnServerNotify?.Invoke($"Session created with code: {relayJoinCode}");
                SubscribeToLobbyEvents(lobby.Id).Forget();
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(
                    allocation.RelayServer.IpV4, 
                    (ushort)allocation.RelayServer.Port, 
                    allocation.AllocationIdBytes, 
                    allocation.Key, 
                    allocation.ConnectionData
                );
                
                NetworkManager.Singleton.StartHost();
                return lobby;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to create lobby and start host: {e}");
                throw; 
            }
        }
        
        private async UniTaskVoid SubscribeToLobbyEvents(string lobbyId)
        {
            var callbacks = new LobbyEventCallbacks();
    
            callbacks.PlayerJoined += (changes) =>
            {
                OnServerNotify?.Invoke($"Player joined!");
                OnServerNotify?.Invoke($"Total players: {_currentLobby.Players.Count + 1}");
                OnSessionFilled?.Invoke();
            };
            try
            {
                await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobbyId, callbacks);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogWarning($"Error subscribing to lobby events: {e.Message}");
            }
        }

        private async UniTask JoinRelayAsClientAsync(Lobby lobby)
        {
            try
            {
                if (!lobby.Data.TryGetValue(LobbyDataKeys.RELAY_JOIN_CODE, out var relayJoinCodeData))
                {
                    throw new InvalidOperationException("Lobby does not contain Relay join code.");
                }

                var relayJoinCode = relayJoinCodeData.Value;

                var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetRelayServerData(
                    joinAllocation.RelayServer.IpV4, 
                    (ushort)joinAllocation.RelayServer.Port, 
                    joinAllocation.AllocationIdBytes, 
                    joinAllocation.Key, 
                    joinAllocation.ConnectionData, 
                    joinAllocation.HostConnectionData
                );
                
                NetworkManager.Singleton.StartClient();
                OnServerNotify?.Invoke($"Session joined with code: {relayJoinCode}");
                OnServerNotify?.Invoke($"Total players: {_currentLobby.Players.Count}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to join Relay as client: {e}");
                throw; 
            }
        }
        
        private void StartHostHeartbeat(CancellationToken externalToken)
        {
            _heartbeatCts?.Cancel();
            _heartbeatCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
            HostLobbyHeartbeatAsync(_heartbeatCts.Token).Forget();
        }
        
        private async UniTask HostLobbyHeartbeatAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && _currentLobby != null)
            {
                try
                {
                    await LobbyService.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
                    await UniTask.Delay(HEARTBEAT_INTERVAL_MS, cancellationToken: cancellationToken);
                }
                catch (Exception) { break; }
            }
        }
        
        public void StartGame()
        {
            LockLobbyAsync(_currentLobby.Id).Forget();
            NetworkManager.Singleton.SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
        }

        private async UniTask LockLobbyAsync(string lobbyId)
        {
            try
            {
                var updateOptions = new UpdateLobbyOptions { IsLocked = true };
                await LobbyService.Instance.UpdateLobbyAsync(lobbyId, updateOptions);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogWarning($"Failed to lock lobby: {e}");
            }
        }

        private Player GetLocalPlayer()
        {
            return new Player(
                id: AuthenticationService.Instance.PlayerId,
                data: new Dictionary<string, PlayerDataObject>() 
            );
        }

        private async UniTask ShutdownAndCleanupAsync()
        {
            _heartbeatCts?.Cancel();
            await LeaveLobbyAsync();

            if (NetworkManager.Singleton && NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.Shutdown();
            }
            
            _currentLobby = null;
            _isJoining = false;
        }

        private async UniTask LeaveLobbyAsync()
        {
            if (_currentLobby == null) return;
            
            try
            {
                var lobbyId = _currentLobby.Id;
                var playerId = AuthenticationService.Instance.PlayerId;

                if (_currentLobby.HostId == playerId)
                {
                    await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogWarning($"Failed to leave or delete lobby. It might have been handled already. Details: {e.Message}");
            }
            finally
            {
                _currentLobby = null;
            }
        }
        private async void OnNetworkStopped(bool obj)
        {
            await ShutdownAndCleanupAsync();
        }

        private void OnDestroy()
        {
            ShutdownAndCleanupAsync().Forget();
        }

        public void NotifyCancel()
        {
            string role = _currentLobby == null ? "Player" : 
                (_currentLobby.HostId == AuthenticationService.Instance.PlayerId ? "Host" : "Client");

            OnServerMessage?.Invoke($"{role} cancelled matchmaking.");
        }
    }
}