using System;
using System.Threading;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;
using Xenocode.Features.Matchmaking.Scripts.Domain.Services;

namespace Xenocode.Features.Matchmaking.Scripts.Domain.Presentation
{
    public class MatchMakingPresenter
    {
        private readonly IMatchMakingView _view;
        private readonly MatchmakingService _matchmakingService;
        
        private CancellationTokenSource _matchmakingCts;

        public MatchMakingPresenter(IMatchMakingView view, MatchmakingService matchmakingService)
        {
            _view = view;
            _matchmakingService = matchmakingService;
            
            SubscribeToServices();
            SubscribeToViewEvents();
            InitializeView();
        }

        private void SubscribeToServices()
        {
            _matchmakingService.OnSessionFilled += HandleSessionFilled;
        }

        private void HandleSessionFilled()
        {
            _view.DisplayStartButton(); 
            _view.HideWaitingForPlayers();
        }

        private void InitializeView()
        {
            Dispose();
            _view.HideCancelButton();
        }

        private void SubscribeToViewEvents()
        {
            _view.OnMatchMakingButtonPressed().AddListener(HandleMatchMakingButtonPressed);
            _view.OnCancelMatchMakingButtonPressed().AddListener(HandleCancelMatchMakingButtonPressed); 
            _view.OnCloseButtonPressed().AddListener(HandleCloseButtonPressed);
            _view.OnStartButtonPressed().AddListener(HandleStartButtonPressed);
        }

        private void HandleStartButtonPressed()
        {
            _matchmakingService.StartGame();
        }

        private void HandleCloseButtonPressed()
        {
            HandleCancelMatchMakingButtonPressed();
            _view.HideChatPanel();
        }

        private void HandleCancelMatchMakingButtonPressed()
        {
            _matchmakingService.NotifyCancel();
            _matchmakingCts?.Cancel();
            ResetToInitialState();
        }

        private async void HandleMatchMakingButtonPressed()
        {
          
            if (_matchmakingCts is { IsCancellationRequested: false }) return;
            
            _matchmakingCts = new CancellationTokenSource();
            
            UpdateView();

            try
            {
                await _matchmakingService.CreateOrJoinLobbyAsync(_matchmakingCts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Matchmaking process was cancelled.");
                ResetToInitialState();
            }
            catch (Exception e)
            {
                Debug.LogError($"An error occurred during matchmaking: {e.Message}");
                ResetToInitialState();
            } 
        }

        private void UpdateView()
        {
            _view.DisplayWaitingForPlayers();
            _view.HideMatchMakingButton();
            _view.DisplayCancelButton();
        }

        private void ResetToInitialState()
        {
            if (_matchmakingCts != null)
            {
                _matchmakingCts.Dispose();
                _matchmakingCts = null;
            }
            
            _view.HideWaitingForPlayers();
            _view.ShowMatchMakingButton();
            _view.HideCancelButton();
            _view.HideStartButton();
        }

        public void Dispose()
        {
            _matchmakingCts?.Dispose();
        }
    }
}