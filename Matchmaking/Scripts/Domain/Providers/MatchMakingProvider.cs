using Xenocode.Features.Matchmaking.Scripts.Delivery;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;
using Xenocode.Features.Matchmaking.Scripts.Domain.Presentation;
using Xenocode.Features.Matchmaking.Scripts.Domain.Services;

namespace Xenocode.Features.Matchmaking.Scripts.Domain.Providers
{
    public static class MatchMakingProvider
    {
        private static MatchmakingService _matchmakingService;
        public static void Present(IMatchMakingView view) => new MatchMakingPresenter(view, GetMatchmakingService());
        public static MatchmakingService GetMatchmakingService()=> _matchmakingService ??= new MatchmakingService();
    }
}