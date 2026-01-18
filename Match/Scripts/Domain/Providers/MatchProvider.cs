using Xenocode.Features.Match.Scripts.Domain.Infrastructure;
using Xenocode.Features.Match.Scripts.Domain.Model;
using Xenocode.Features.Match.Scripts.Domain.Presentation;
using Xenocode.Features.Match.Scripts.Domain.Services;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;

namespace Xenocode.Features.Match.Scripts.Domain.Providers
{
    public static class MatchProvider
    {
        private static TeamsRepository _teamsRepository;
        private static IPlayerRepository _playerRepository;
        private static MatchService _service;
        public static MatchPresenter Present(IMatchView view) => new(view,GetMatchService());

        public static MatchService GetMatchService() =>
            _service ??= new MatchService(GetTeamsRepository(), GetPlayerRepository());

        private static ITeamsRepository GetTeamsRepository() => _teamsRepository ??= new TeamsRepository();
        
        public static IPlayerRepository GetPlayerRepository() => _playerRepository ??= new PlayerRepository();
    }
}
