using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.MatchStatus.Scripts.Domain.Model;
using Xenocode.Features.MatchStatus.Scripts.Domain.Provider;

namespace Xenocode.Features.MatchStatus.Scripts.Delivery
{
    public class MatchStatusView : MonoBehaviour, IMatchStatusView
    {
        [Header("Healthbars UI")]
        [SerializeField] private GameObject _classicUIPanel;
        [SerializeField] private GameObject _sharedUIPanel;
        [SerializeField] private UnityEngine.UI.Slider _player1Slider;
        [SerializeField] private UnityEngine.UI.Slider _player2Slider;
        [SerializeField] private UnityEngine.UI.Slider _sharedSlider;
        [SerializeField] private GameObject _victoryPanel;

        void Start()
        {
            if (!NetworkManager.Singleton.IsClient)
            {
                gameObject.SetActive(false);
                return;
            }
            MatchStatusProvider.Present(this);
        }
        
        public void ToggleHealthLayout()
        {
            bool isClassicActive = _classicUIPanel.activeSelf;
            _classicUIPanel.SetActive(!isClassicActive);
            _sharedUIPanel.SetActive(isClassicActive);
        }

        public void UpdateIndividualBar(int teamIndex, float currentHealth, float maxHealth)
        {
            float ratio = currentHealth / maxHealth;
            if (teamIndex == 0) 
                _player1Slider.value = ratio;
            else 
                _player2Slider.value = ratio;
        }

        public void UpdateSharedBar(float teamAHealth, float teamBHealth)
        {
            float totalCurrent = teamAHealth + teamBHealth;
            
            if (totalCurrent <= 0) 
            {
                _sharedSlider.value = 0.5f;
                return;
            }
            
            _sharedSlider.value = teamBHealth / totalCurrent;
        }

        public void ShowVictory()
        {
            throw new System.NotImplementedException();
        }
    }
}