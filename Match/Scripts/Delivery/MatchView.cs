using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.BuildCreator.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Delivery;
using Xenocode.Features.Match.Scripts.Domain.Model;
using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.Teams.Scripts.Domain.Model;
using Xenocode.Features.UserSupplies.Scripts.Delivery;

namespace Xenocode.Features.Match.Scripts.Delivery
{
    public class MatchView : MonoBehaviour, IMatchView
    {
        [SerializeField] private GameObject _matchCountdown;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _castlePrefab;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private GameObject _suppliesManagerPrefab;
        
        private UnityEvent _onAppear = new();

        private void Awake()
        {
            MatchProvider.Present(this);
        }

        private void Start()
        {
            _onAppear.Invoke();
        }

        public UnityEvent OnAppear() => _onAppear;

        public async UniTask ShowCountdown()
        {
            _matchCountdown.SetActive(true);
            await UniTask.WaitForSeconds(5);
            _matchCountdown.SetActive(false);
        }
        
        public Dictionary<ulong, UserSuppliesNet> CreatePlayers(Dictionary<ulong, Team> clientToTeamMap)
        {
            var entities = new Dictionary<ulong, UserSuppliesNet>();
            foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                var playerInstance = Instantiate(_playerPrefab);
                playerInstance.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, true);
        
                if(playerInstance.TryGetComponent<UserSuppliesNet>(out var supplies))
                {
                    entities.Add(clientId, supplies);
                }
        
                if (clientToTeamMap.TryGetValue(clientId, out var playerTeam))
                {
                    playerInstance.GetComponent<IBuildingCreatorView>().SetTeam(playerTeam);
                }
            }
            return entities;
        }

        public void CreateTeamBase(Vector3 baseSpawnPoint, Team team)
        {
            var instance = Instantiate(_castlePrefab);
            instance.transform.position = baseSpawnPoint;
            instance.transform.LookAt(Vector3.zero);
            instance.GetComponent<NetworkObject>().Spawn();
            instance.GetComponent<CastleController>().Initialize(team);
        }

        public void SetCameraInitialPosition(Vector3 castleSpawnPoint)
        {
            _cameraTransform.position = new Vector3(castleSpawnPoint.x, _cameraTransform.position.y, castleSpawnPoint.z -15);
        }

        public void CreateSuppliesManager()
        {
            var instance = Instantiate(_suppliesManagerPrefab);
            if (instance.TryGetComponent<NetworkObject>(out var netObj))
            {
                netObj.Spawn(); 
            }
        }
    }
}