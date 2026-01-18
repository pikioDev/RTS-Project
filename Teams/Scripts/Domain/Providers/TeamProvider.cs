using Xenocode.Features.Teams.Scripts.Domain.Services;

namespace Xenocode.Features.Teams.Scripts.Domain.Providers
{
    public static class TeamProvider
    {
        private static TeamService _teamService;
        public static TeamService GetTeamService() => _teamService ??= new TeamService();
    }
}