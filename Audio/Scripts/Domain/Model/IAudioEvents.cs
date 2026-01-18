using System;
using UnityEngine;

namespace Xenocode.Features.Audio.Scripts.Domain.Model
{
    public interface IAudioEvents 
    {
        event Action<string, int, Vector3> OnSoundRequested;

        void PlaySound(string profileId, int type, Vector3 position);
    }
}