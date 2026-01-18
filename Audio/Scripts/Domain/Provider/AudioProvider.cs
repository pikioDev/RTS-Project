using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Infrastructure;
using Xenocode.Features.Audio.Scripts.Domain.Model;
using Xenocode.Features.Audio.Scripts.Domain.Services;

namespace Xenocode.Features.Audio.Scripts.Domain.Provider
{
    public static class AudioProvider
    {   
        private static AudioService _audioService;
        private static IAudioEvents _audioEvents;
        
        public static void InitializeService() => _audioService ??= new AudioService(GetAudioRepository(), GetAudioEvents());
        
        public static IAudioEvents GetAudioEvents() => _audioEvents ??= new AudioEvents();
        
        private static AudioRepository GetAudioRepository() => Resources.Load<AudioRepository>("AudioRepository");
    }
}