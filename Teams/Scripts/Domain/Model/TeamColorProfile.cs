using System;
using UnityEngine;

namespace Xenocode.Features.Teams.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "TeamColorProfile", menuName = "Game/Team Color Profile")]
    public class TeamColorProfile : ScriptableObject
    {
        [Tooltip("Color principal para el Equipo A")]
        public Color TeamA_Color = Color.red;

        [Tooltip("Color principal para el Equipo B")]
        public Color TeamB_Color = Color.blue;

        public Color GetColor(Team team)
        {
            switch (team)
            {
                case Team.TeamA: 
                    return TeamA_Color;
                case Team.TeamB:
                    return TeamB_Color;
                default:
                    throw new ArgumentOutOfRangeException(nameof(team), team, null);
            }
        }
    }
}