using System;
using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "Castle", menuName = "Xenocode/Castle/CastleSpawnPoints")]
    public class CastleSpawnPointAsset : ScriptableObject
    {
        public List<Vector3> SpawnPoints = new();
 
        public Vector3 GetTeamSpawnPoint(Team team)
        {
            switch (team)
            {
                case Team.TeamA: return SpawnPoints[0];
                case Team.TeamB: return SpawnPoints[1];
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }
    }
}