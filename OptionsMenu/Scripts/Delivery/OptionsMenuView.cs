using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xenocode.Features.OptionsMenu.Domain.Model;
using Xenocode.Features.OptionsMenu.Domain.Providers;

namespace Xenocode.Features.OptionsMenu.Delivery
{
    public class OptionsMenuView : MonoBehaviour, IOptionsMenuView
    {
        [SerializeField] private Button _screenOptionsButton;
        [SerializeField] private Button _audioOptionsButton;
        [SerializeField] private GameObject _screenOptionsPanel;
        [SerializeField] private GameObject _audioOptionsPanel;
        private GameObject _openedPanel;
        public UnityEvent OnScreenOptionsPressed() => _screenOptionsButton.onClick;
        public UnityEvent OnAudioOptionsPressed() => _audioOptionsButton.onClick;
        public void ShowScreenOptions()
        {
            CloseLastPanel();
            _screenOptionsPanel.SetActive(true);
            _openedPanel = _screenOptionsPanel;
        }
        public void ShowAudioOptions()
        {
            CloseLastPanel();
            _audioOptionsPanel.SetActive(true);
            _openedPanel = _audioOptionsPanel;
        }
        private void CloseLastPanel()
        {
            _openedPanel.SetActive(false);
        }
        void Start()
        {
            OptionsMenuProvider.Present(this);
            _openedPanel = _screenOptionsPanel;
        }
    }
}