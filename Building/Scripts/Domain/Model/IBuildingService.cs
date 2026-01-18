using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Model
{
    public interface IBuildingService
    {
        public UnityEvent OnUnitReadyToSpawn();
        public void Tick();
    }
}