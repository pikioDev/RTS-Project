using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.BuildCreator.Scripts.Domain.Model
{
    public interface IBuildingPlacementService
    {
        Vector3 GetSnappedPosition(Vector3 rawPosition, BuildingType type);

        public bool IsValidPlacement(Vector3 position, Team team);

        BuildingsDatabaseSO GetBuildData();
    }
}