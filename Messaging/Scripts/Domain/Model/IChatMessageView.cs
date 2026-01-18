using System;

namespace Xenocode.Features.Messaging.Scripts.Domain.Model
{
    public interface IChatView
    {
        event Action<string, MessageChannel> OnMessageSubmit;
        void DisplayChatMessage(string message);
        void ClearChatInput();
    }
}