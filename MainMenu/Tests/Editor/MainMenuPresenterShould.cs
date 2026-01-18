using NSubstitute;
using NUnit.Framework;
using UnityEngine.Events;
using Xenocode.Features.MainMenu.Scripts.Domain.Model;
using Xenocode.Features.MainMenu.Scripts.Domain.Presenter;

namespace Xenocode.Features.MainMenu.Tests.Editor
{
    public class MainMenuPresenterShould
    {
        private readonly UnityEvent _onMultiplayerPanelDisplayed = new UnityEvent();
        private readonly UnityEvent _onMultiplayerPanelClosed = new UnityEvent();
        private readonly UnityEvent _onOptionsMenuOpened = new UnityEvent();
        private readonly UnityEvent _onOptionsMenuClosed = new UnityEvent();
        private readonly UnityEvent _onExitButtonPressed = new UnityEvent();
        private IMainMenuView _view;
        
        [SetUp]
        public void Setup()
        {
            _view = Substitute.For<IMainMenuView>();
            _view.OnMultiplayerOpened().Returns(_onMultiplayerPanelDisplayed);
            _view.OnMultiplayerClosed().Returns(_onMultiplayerPanelClosed);
            _view.OnOptionsMenuOpened().Returns(_onOptionsMenuOpened);
            _view.OnOptionsMenuClosed().Returns(_onOptionsMenuClosed);
            _view.OnExitButtonPressed().Returns(_onExitButtonPressed);
            GivenAPresenter();
        }
        
        [Test]
        public void DisplayMultiplayerPanel()
        {
            WhenOpenMultiplayerPanelIsTriggered();
            ThenMultiplayerPanelIsDisplayed();
        }

        [Test]
        public void CloseMultiplayerPanel()
        {
            WhenCloseMultiplayerPanelIsTriggered();
            ThenMultiplayerPanelIsClosed();
        }

        [Test]
        public void DisplayOptionsMenu()
        {
            WhenOpenOptionsMenuPanelIsTriggered();
            ThenOptionsMenuPanelIsOpened();
        }

        [Test]
        public void CloseOptionsMenu()
        {
            WhenCloseOptionsMenuPanelIsTriggered();
            ThenOptionsMenuPanelIsClosed();
        }

        [Test]
        public void CloseApplication()
        {
            WhenCloseApplicationIsTriggered();
            ThenApplicationIsClosed();
        }
        private void GivenAPresenter()
        {
            var presenter = new MainMenuPresenter(_view);
        }
        
        private void WhenCloseApplicationIsTriggered() => _onExitButtonPressed.Invoke();

        private void WhenCloseOptionsMenuPanelIsTriggered() => _onOptionsMenuClosed.Invoke();

        private void WhenOpenOptionsMenuPanelIsTriggered() => _onOptionsMenuOpened.Invoke();

        private void WhenCloseMultiplayerPanelIsTriggered() => _onMultiplayerPanelClosed.Invoke();

        private void WhenOpenMultiplayerPanelIsTriggered() => _onMultiplayerPanelDisplayed.Invoke();

        private void ThenMultiplayerPanelIsDisplayed() => _view.Received(1).ShowMultiplayer();

        private void ThenMultiplayerPanelIsClosed() => _view.Received(1).HideMultiplayer();

        private void ThenOptionsMenuPanelIsOpened() => _view.Received(1).ShowOptions();

        private void ThenOptionsMenuPanelIsClosed() => _view.Received(1).HideOptions();
        
        private void ThenApplicationIsClosed() => _view.Received(1).CloseApplication();
    }
}