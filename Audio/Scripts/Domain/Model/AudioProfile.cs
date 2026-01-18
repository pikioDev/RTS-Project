using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Xenocode.Features.Audio.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "NewAudioProfile", menuName = "Xenocode/Audio/Audio Profile")]
    public class AudioProfile : ScriptableObject
    {
        [Serializable]
        public struct AudioData
        {
            public AudioClipType clipType;
            public List<AudioClip> clips;

            public AudioClip GetRandom() => (clips == null || clips.Count == 0) ? null : clips[UnityEngine.Random.Range(0, clips.Count)];
        }

        [SerializeField] private List<AudioData> _audioEntries;
        private Dictionary<AudioClipType, AudioData> _cache;

        public AudioClip GetAudioClip(AudioClipType clipType)
        {
            _cache ??= _audioEntries.ToDictionary(e => e.clipType, e => e);
            return _cache.TryGetValue(clipType, out var data) ? data.GetRandom() : null;
        }
        
        private void OnValidate() => _cache = null;
    }
}