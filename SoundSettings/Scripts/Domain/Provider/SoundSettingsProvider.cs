using UnityEngine;
using UnityEngine.Audio;
using Xenocode.Features.PlayerPref.Scripts.Domain.Providers;
using Xenocode.Features.SoundSettings.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Presenter;
using Xenocode.Features.SoundSettings.Scripts.Domain.Services;

namespace Xenocode.Features.SoundSettings.Scripts.Domain.Providers
    {
        public static class SoundSettingsProvider
        {
            private static IAudioMixerService _audioMixerService;
            
            public static void Present(ISoundSettingsView view) =>
                new SoundSettingsPresenter(view, GetAudioMixerService());
            
            private static IAudioMixerService GetAudioMixerService() =>
                _audioMixerService ??= new AudioMixerService(SettingsRepositoryProvider.GetSettingsRepository(), GetMixerWrapper());
            
            private static IAudioMixerWrapper GetMixerWrapper()
            {
                var mainMixer = Resources.Load<AudioMixer>("MainMixer");
                return new AudioMixerWrapper(mainMixer);
            }
        }

        internal class AudioMixerWrapper : IAudioMixerWrapper
        {
            private readonly AudioMixer _mixer;
            
            public AudioMixerWrapper(AudioMixer mixer)
            {
                _mixer = mixer;
            }
            public void SetFloat(string name, float value) => _mixer.SetFloat(name, value);
        }
    }