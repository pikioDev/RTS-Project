using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.MatchStatus.Scripts.Delivery;
using Xenocode.Features.MatchStatus.Scripts.Domain.Presentation;

namespace Xenocode.Features.MatchStatus.Scripts.Domain.Provider
{
    internal static class MatchStatusProvider
    {
        public static void Present(MatchStatusView view) => 
            new MatchStatusPresenter(view, MatchProvider.GetMatchService());
    }
}