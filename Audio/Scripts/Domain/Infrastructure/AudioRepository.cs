using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;

namespace Xenocode.Features.Audio.Scripts.Domain.Infrastructure
{
    [CreateAssetMenu(fileName = "AudioRepository", menuName = "Xenocode/Audio/Audio Repository")]
    public class AudioRepository : ScriptableObject
    {
        [SerializeField] private List<AudioProfile> _profiles;
        private Dictionary<string, AudioProfile> _cache;

        public AudioClip ResolveClip(string profileId, AudioClipType clipType)
        {
            _cache ??= _profiles.ToDictionary(p => p.name, p => p);
            return _cache.TryGetValue(profileId, out var profile) ? profile.GetAudioClip(clipType) : null;
        }

        private void OnValidate() => _cache = null;
    }
}