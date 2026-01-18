using System;
using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;

namespace Xenocode.Features.Audio.Scripts.Domain.Services
{
    public class AudioEvents : IAudioEvents
    {
        public event Action<string, int, Vector3> OnSoundRequested;

        public void PlaySound(string profileId, int type, Vector3 position)
        {
            OnSoundRequested?.Invoke(profileId, type, position);
        }
    }
}