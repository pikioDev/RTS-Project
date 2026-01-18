using UnityEngine;
using Xenocode.Features.BuildCreator.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Delivery;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.BuildCreator.Scripts.Domain.Service
{
    public class BuildingPlacementService : IBuildingPlacementService
    {
        private readonly BuildingsDatabaseSO _database;
        private const float CellSize = 2f;
        
        public BuildingPlacementService()
        {
            _database = Resources.Load<BuildingsDatabaseSO>("BuildingDatabase");
        }

        public bool IsValidPlacement(Vector3 position, Team team)
        {
            if (!IsWithinTeamZone(position, team)) return false;
            
            var overlap = Physics.OverlapBox(position + Vector3.up * 1.0f, new Vector3(0.9f, 0.9f, 0.9f), Quaternion.identity);
    
            foreach (var col in overlap)
            {
                if (col.gameObject.layer == 7)
                {
                    return false;
                }
            }

            return true;
        }

        public BuildingsDatabaseSO GetBuildData() => _database;

        private bool IsWithinTeamZone(Vector3 position, Team team)
        {
            var colliders = Physics.OverlapBox(position, Vector3.one * 0.1f, Quaternion.identity);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<CastleController>(out var zone))
                    return zone.GetTeam() == team;
            }
            return false;
        }
        
        public Vector3 GetSnappedPosition(Vector3 rawPosition, BuildingType type)
        {
            if (type == BuildingType.None) return rawPosition;
            int width = 2;  
            int length = 2; 
            float cellSize = 1f;
            
            float gridX = Mathf.Floor(rawPosition.x / cellSize) * cellSize;
            float gridZ = Mathf.Floor(rawPosition.z / cellSize) * cellSize;
            
            float offsetX = (width * 0.4f) + (cellSize * 0.4f);
            float offsetZ = (length * 0.4f) + (cellSize * 0.4f);
            
            float finalX = gridX + offsetX;
            float finalZ = gridZ + offsetZ;
            
            float y = rawPosition.y; 
            return new Vector3(finalX, y, finalZ);
        }
        
        public Vector3 GetCastleSpawnPoint(Team team) =>
            Resources.Load<CastleSpawnPointAsset>("CastleSpawnPoints").GetTeamSpawnPoint(team);
    }
}