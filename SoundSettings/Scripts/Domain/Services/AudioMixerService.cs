using UnityEngine;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Model;

namespace Xenocode.Features.SoundSettings.Scripts.Domain.Services
{
    public class AudioMixerService : IAudioMixerService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IAudioMixerWrapper _mixer;
        
        public AudioMixerService(ISettingsRepository settingsRepository,  IAudioMixerWrapper mixer)
        {
            _mixer = mixer;
            _settingsRepository = settingsRepository;
        }
        
        public void InitializeMasterVolume()
        {
            var linearValue = _settingsRepository.GetSettings().MasterVolume;
            ApplyVolume("Master", GetLogarithmicScale(linearValue));
        }
        
        public void InitializeSfxVolume()
        {
            var linearValue = _settingsRepository.GetSettings().SfxVolume;
            ApplyVolume("Sfx", GetLogarithmicScale(linearValue));
        }
        
        public void InitializeMusicVolume()
        {
            var linearValue = _settingsRepository.GetSettings().MusicVolume;
            ApplyVolume("Music", GetLogarithmicScale(linearValue));
        }
        
        public void InitializeToggleMute()
        {
            var isMuted = _settingsRepository.GetSettings().IsMuted;
            var lastMasterVolume = isMuted ? -80 : _settingsRepository.GetSettings().MasterVolume;
            ApplyVolume("Master", GetLogarithmicScale(lastMasterVolume));
        }

        public float GetMasterVolume() => _settingsRepository.GetSettings().MasterVolume;

        public float GetSfxVolume() => _settingsRepository.GetSettings().SfxVolume;

        public float GetMusicVolume() => _settingsRepository.GetSettings().MusicVolume;

        public bool GetMutedState() => _settingsRepository.GetSettings().IsMuted;

        public void SetMasterVolume(float volume)
        {
            _settingsRepository.GetSettings().MasterVolume = volume;
            ApplyVolume("Master", GetLogarithmicScale(volume));
            _settingsRepository.SaveAllSettings();
        }

        public void SetSfxVolume(float volume)
        {
            _settingsRepository.GetSettings().SfxVolume = volume;
            ApplyVolume("Sfx", GetLogarithmicScale(volume));
            _settingsRepository.SaveAllSettings();
        }

        public void SetMusicVolume(float volume)
        {
            _settingsRepository.GetSettings().MusicVolume = volume;
            ApplyVolume("Music", GetLogarithmicScale(volume));
            _settingsRepository.SaveAllSettings();
        }

        public void SetToggleMute(bool isMuted)
        {
            _settingsRepository.GetSettings().IsMuted = isMuted;
            var dbValue = _settingsRepository.GetSettings().MasterVolume;
            ApplyVolume("Master" , GetLogarithmicScale(dbValue));
            _settingsRepository.SaveAllSettings();
        }

        private void ApplyVolume(string group, float dbValue)
        {
            var isMuted = _settingsRepository.GetSettings().IsMuted;
            float targetDb = isMuted ? -80f : dbValue;
            _mixer.SetFloat(group, targetDb); 
        }
        
        public float GetLogarithmicScale(float linearValue) => Mathf.Log10(Mathf.Max(linearValue, 0.0001f)) * 20;
    }
}