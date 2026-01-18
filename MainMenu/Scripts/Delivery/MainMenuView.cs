using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xenocode.Features.MainMenu.Scripts.Domain.Model;
using Xenocode.Features.MainMenu.Scripts.Domain.Provider;

namespace Xenocode.Features.MainMenu.Scripts.Delivery
{
    public class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [Header("Main")] 
        [SerializeField] private Button _openMultiplayerButton;
        [SerializeField] private Button _openOptionsButton;
        [SerializeField] private Button _closedMultiplayerButton;
        [SerializeField] private Button _closeOptionButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _matchPanel;


        public UnityEvent OnMultiplayerOpened() => _openMultiplayerButton.onClick;
        public UnityEvent OnMultiplayerClosed() => _closedMultiplayerButton.onClick;
        public UnityEvent OnOptionsMenuOpened() => _openOptionsButton.onClick;
        public UnityEvent OnOptionsMenuClosed() => _closeOptionButton.onClick;
        public UnityEvent OnExitButtonPressed() => _exitButton.onClick;
        
        public void ShowOptions() => _optionsPanel.SetActive(true);
        public void HideOptions() => _optionsPanel.SetActive(false);
        public void ShowMultiplayer() => _matchPanel.SetActive(true);
        public void HideMultiplayer() => _matchPanel.SetActive(false); 
        public void CloseApplication()
        {
            Application.Quit();
        }

        void Start()
        {
            MainMenuProvider.Present(this);
        }
    }
}