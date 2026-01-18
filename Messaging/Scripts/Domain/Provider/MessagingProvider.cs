using Xenocode.Features.Matchmaking.Scripts.Domain.Providers;
using Xenocode.Features.Messaging.Scripts.Domain.Model;
using Xenocode.Features.Messaging.Scripts.Domain.Presentation;
using Xenocode.Features.Messaging.Scripts.Domain.Services;

namespace Xenocode.Features.Messaging.Scripts.Domain.Provider
{
    public static class MessagingProvider
    {
        private static IMessagingService _messagingService;

        public static void Present(INetworkChat view) => new ChatPresenter(view, GetMessagingService(), MatchMakingProvider.GetMatchmakingService());

        private static IMessagingService GetMessagingService() => _messagingService ??= new MessagingService();
    }
}