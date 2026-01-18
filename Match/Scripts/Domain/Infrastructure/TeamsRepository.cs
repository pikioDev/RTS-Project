using System.Collections.Generic;
using Xenocode.Features.Teams;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Match.Scripts.Domain.Infrastructure
{
    public class TeamsRepository : ITeamsRepository
    {
        private Dictionary<Team, List<ulong>> _teams = new();

        public void Set(Dictionary<Team, List<ulong>> teams)
        {
            _teams = teams;
        }

        public Dictionary<Team, List<ulong>> GetTeams() => _teams;
    }
}