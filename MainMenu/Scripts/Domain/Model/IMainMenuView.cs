using UnityEngine.Events;

namespace Xenocode.Features.MainMenu.Scripts.Domain.Model
{
    public interface IMainMenuView
    {
        UnityEvent OnMultiplayerOpened();
        UnityEvent OnMultiplayerClosed();
        UnityEvent OnOptionsMenuOpened();
        UnityEvent OnOptionsMenuClosed();
        UnityEvent OnExitButtonPressed();
        void ShowOptions();
        void HideOptions();
        void ShowMultiplayer();
        void HideMultiplayer();
        void CloseApplication();
    }
}
