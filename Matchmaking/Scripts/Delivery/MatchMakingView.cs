using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;
using Xenocode.Features.Matchmaking.Scripts.Domain.Providers;

namespace Xenocode.Features.Matchmaking.Scripts.Delivery
{
    public class MatchMakingView : MonoBehaviour, IMatchMakingView
    {
        [SerializeField] private Button _matchmakingButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _startButton;
        [SerializeField] private GameObject waitingForPlayersPopup;

        void Awake()
        {
            MatchMakingProvider.Present(this);
            HideStartButton();
        }
        
        public UnityEvent OnMatchMakingButtonPressed() => _matchmakingButton.onClick;
        public UnityEvent OnCancelMatchMakingButtonPressed() => _cancelButton.onClick;
        public UnityEvent OnCloseButtonPressed() => _closeButton.onClick;
        public UnityEvent OnStartButtonPressed() => _startButton.onClick;
        public void DisplayWaitingForPlayers() => waitingForPlayersPopup.SetActive(true);
        public void HideWaitingForPlayers() => waitingForPlayersPopup.SetActive(false);
        public void HideMatchMakingButton() => _matchmakingButton.gameObject.SetActive(false);
        public void ShowMatchMakingButton() => _matchmakingButton.gameObject.SetActive(true);
        public void DisplayCancelButton() => _cancelButton.interactable = true;
        public void HideCancelButton() => _cancelButton.interactable = false;
        public void HideStartButton() => _startButton.gameObject.SetActive(false);
        public void DisplayStartButton()
        {
            _startButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);
        }

        public void HideChatPanel() => gameObject.SetActive(false);
    }
}