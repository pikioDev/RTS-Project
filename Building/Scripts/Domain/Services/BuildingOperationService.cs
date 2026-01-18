using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Services
{
    public class BuildingOperationService : IBuildingService
    {
        private readonly UnityEvent _onUnitReadyToSpawn = new();
        private readonly BuildingProfileSo _profile;
        private float _timer;
        
        public UnityEvent OnUnitReadyToSpawn() => _onUnitReadyToSpawn;
        public float GetTimer() => _timer;
        
        public BuildingOperationService(BuildingProfileSo profile)
        {
            _profile = profile; 
            _timer = profile.GetSpawnInterval();
        }
        
        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = _profile.GetSpawnInterval();
                _onUnitReadyToSpawn?.Invoke();
            }
        }
    }
}