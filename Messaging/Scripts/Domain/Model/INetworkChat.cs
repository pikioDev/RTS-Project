using System;

namespace Xenocode.Features.Messaging.Scripts.Domain.Model
{
    public interface INetworkChat
    {
        event Action<MessageData> OnMessageReceived;
        IChatView GetView();
        public void SendRequest(string content, MessageChannel channel);
    }
}