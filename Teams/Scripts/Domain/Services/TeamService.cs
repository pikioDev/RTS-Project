using UnityEngine;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Teams.Scripts.Domain.Services
{
    public class TeamService
    {
        public Color GetTeamColor(Team team) =>
            Resources.Load<TeamColorProfile>("TeamColorProfile").GetColor(team);
    }
}