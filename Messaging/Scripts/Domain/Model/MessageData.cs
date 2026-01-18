namespace Xenocode.Features.Messaging.Scripts.Domain.Model
{
    public struct MessageData
    {
        public readonly string Sender;
        public readonly string Content;
        public readonly MessageChannel Channel; 
    
        public MessageData(string sender, string content, MessageChannel channel)
        {
            Sender = sender;
            Content = content;
            Channel = channel;
        }
    }
    
    public enum MessageChannel { All, Team}
}
