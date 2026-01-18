using System;

namespace Xenocode.Features.Messaging.Scripts.Domain.Model
{
    public interface IMessagingService
    {
        event Action<MessageData> OnMessageReceived;
        
        event Action<string, MessageChannel> OnRequestSendMessage;

        void Send(string content, MessageChannel channel = MessageChannel.All);
        void Receive(MessageData message);
    }
}