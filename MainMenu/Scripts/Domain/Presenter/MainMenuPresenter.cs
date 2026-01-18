using Xenocode.Features.MainMenu.Scripts.Domain.Model;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;

namespace Xenocode.Features.MainMenu.Scripts.Domain.Presenter
{
    public class MainMenuPresenter
    {
        private readonly IMainMenuView _view;
        private PlayerSettings _currentSettings;
        public MainMenuPresenter(IMainMenuView view)
        {
            _view = view;
            SubscribeToViewEvents();
        }
        private void SubscribeToViewEvents()
        {
            _view.OnMultiplayerOpened().AddListener(HandleMultiplayerOpened);
            _view.OnMultiplayerClosed().AddListener(HandleMultiplayerClosed);
            _view.OnOptionsMenuOpened().AddListener(HandleOptionsMenuOpened);
            _view.OnOptionsMenuClosed().AddListener(HandleOptionsMenuClosed);
            _view.OnExitButtonPressed().AddListener(HandleApplicationQuit);
        }

        private void HandleMultiplayerOpened() => _view.ShowMultiplayer();
        private void HandleMultiplayerClosed() => _view.HideMultiplayer();
        private void HandleOptionsMenuOpened() => _view.ShowOptions();
        private void HandleOptionsMenuClosed() => _view.HideOptions();
        private void HandleApplicationQuit() => _view.CloseApplication();
    }
    
}