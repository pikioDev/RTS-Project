using UnityEngine.Events;

namespace Xenocode.Features.Matchmaking.Scripts.Domain.Model
{
    public interface IMatchMakingView
    {
        UnityEvent OnMatchMakingButtonPressed();
        UnityEvent OnCancelMatchMakingButtonPressed();
        UnityEvent OnCloseButtonPressed();
        UnityEvent OnStartButtonPressed();
        void DisplayWaitingForPlayers();
        void HideMatchMakingButton();
        void ShowMatchMakingButton(); 
        void DisplayCancelButton(); 
        void HideCancelButton();
        void HideWaitingForPlayers();
        void HideStartButton();
        void DisplayStartButton();
        public void HideChatPanel();
    }
}