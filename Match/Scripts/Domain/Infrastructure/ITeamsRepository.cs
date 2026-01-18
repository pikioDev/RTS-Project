using System.Collections.Generic;
using Xenocode.Features.Teams;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Match.Scripts.Domain.Infrastructure
{
    public interface ITeamsRepository
    {
        void Set(Dictionary<Team, List<ulong>> teams);
        Dictionary<Team, List<ulong>>  GetTeams();
    }
}