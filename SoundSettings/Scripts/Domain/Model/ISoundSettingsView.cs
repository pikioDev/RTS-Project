using UnityEngine.Events;

namespace Xenocode.Features.SoundSettings.Scripts.Domain.Model
{
    public interface ISoundSettingsView
    {
        UnityEvent<bool> OnMuteToggleChanged();
        UnityEvent<float> OnMasterSliderChanged();
        UnityEvent<float> OnSfxSliderChanged();
        UnityEvent<float> OnMusicSliderChanged();
        public void SetMasterSlider(float volume){}
        public void SetSfxSlider(float volume){}
        public void SetMusicSlider(float volume){}
        public void SetMuteToggle(bool isMuted){}
    }
}