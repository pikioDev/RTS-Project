using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xenocode.Features.SoundSettings.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Providers;

namespace Xenocode.Features.SoundSettings.Scripts.Delivery
{
    public class SoundSettingsView : MonoBehaviour, ISoundSettingsView
    {
        [SerializeField] private Toggle _muteToggle;
        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        public UnityEvent<float> OnMasterSliderChanged() => _masterSlider.onValueChanged;
        public UnityEvent<float> OnSfxSliderChanged() => _sfxSlider.onValueChanged;
        public UnityEvent<float> OnMusicSliderChanged() => _musicSlider.onValueChanged;
        public UnityEvent<bool> OnMuteToggleChanged() => _muteToggle.onValueChanged;
        
        public void SetMasterSlider(float volume) => _masterSlider.SetValueWithoutNotify(volume);
        public void SetSfxSlider(float volume) => _sfxSlider.SetValueWithoutNotify(volume);
        public void SetMusicSlider(float volume) => _musicSlider.SetValueWithoutNotify(volume);
        public void SetMuteToggle(bool isMuted) => _muteToggle.SetIsOnWithoutNotify(isMuted);
        
        void Start()
        {
            SoundSettingsProvider.Present(this);
        }       
    }
}