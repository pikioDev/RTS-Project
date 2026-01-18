using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Infrastructure;
using Xenocode.Features.Audio.Scripts.Domain.Model;

namespace Xenocode.Features.Audio.Scripts.Domain.Services
{
    public class AudioService
    {
        private readonly AudioRepository _repository;
        private readonly IAudioEvents _events;

        public AudioService(AudioRepository repository, IAudioEvents events)
        {
            _repository = repository;
            _events = events;
            _events.OnSoundRequested += HandleSoundRequest;
        }

        private void HandleSoundRequest(string id, int type, Vector3 pos) 
        {
            AudioSource.PlayClipAtPoint(_repository.ResolveClip(id, (AudioClipType)type), pos);
        }
    }
}