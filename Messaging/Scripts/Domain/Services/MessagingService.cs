using System;
using Xenocode.Features.Messaging.Scripts.Domain.Model;

namespace Xenocode.Features.Messaging.Scripts.Domain.Services
{
    public class MessagingService : IMessagingService
    {
        public event Action<MessageData> OnMessageReceived;
        
        public event Action<string, MessageChannel> OnRequestSendMessage;

        public void Send(string content, MessageChannel channel = MessageChannel.All) => 
            OnRequestSendMessage?.Invoke(content, channel);

        public void Receive(MessageData message) => OnMessageReceived?.Invoke(message);
    }
}