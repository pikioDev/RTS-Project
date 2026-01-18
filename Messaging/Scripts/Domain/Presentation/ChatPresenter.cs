using Xenocode.Features.Matchmaking.Scripts.Domain.Services;
using Xenocode.Features.Messaging.Scripts.Domain.Model;

namespace Xenocode.Features.Messaging.Scripts.Domain.Presentation
{
    public class ChatPresenter 
    {
        private readonly INetworkChat _chat;
        private readonly IChatView _view;
        private readonly IMessagingService _messagingService;
        private readonly MatchmakingService _matchmakingService;
        
        public ChatPresenter(INetworkChat chat, IMessagingService messagingService, MatchmakingService matchmakingService)
        {
            _chat = chat;
            _view = _chat.GetView();
            _messagingService = messagingService;
            _matchmakingService = matchmakingService;
        
            SubscribeToViewEvents();
            SubscribeToMessagingService();
            SubscribeToMatchNotifications();
        }
        private void SubscribeToViewEvents()
        {
            _view.OnMessageSubmit += HandleMessageSubmit;
        }

        private void SubscribeToMessagingService()
        {
            _chat.OnMessageReceived += _messagingService.Receive; 
            _messagingService.OnMessageReceived += HandleShowText;
            _messagingService.OnRequestSendMessage += _chat.SendRequest;
        }
        
        private void SubscribeToMatchNotifications()
        {
            _matchmakingService.OnServerNotify += HandleServerNotify;
            _matchmakingService.OnServerMessage += HandleServerMessage;
        }

        private void HandleServerMessage(string content)
        {
            _messagingService.Send(content);
        }

        private void HandleServerNotify(string content)
        {
            _view.DisplayChatMessage(content);
        }

        private void HandleMessageSubmit(string content, MessageChannel channel)
        { 
            _messagingService.Send(content, channel);
            _view.ClearChatInput();
        }

        private void HandleShowText(MessageData chatMessage)
        {
            var color = chatMessage.Channel == MessageChannel.Team ? "green" : "orange";
            var formattedMessage = $"<color={color}><b>{chatMessage.Sender}:</b></color> {chatMessage.Content}";
            _view.DisplayChatMessage(formattedMessage);
        }
    }
}