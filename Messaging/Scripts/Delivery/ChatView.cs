using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xenocode.Features.Messaging.Scripts.Domain.Model;

namespace Xenocode.Features.Messaging.Scripts.Delivery
{
    public class ChatView: MonoBehaviour, IChatView
    {
        [SerializeField] private TMP_InputField _chatInputField;
        [SerializeField] private TextMeshProUGUI _chatContentText;
        [SerializeField] private Button _sendButton;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private MessageChannel _messageChannel = MessageChannel.All;

        public event Action<string, MessageChannel> OnMessageSubmit;

        private void Start()
        {
            _sendButton.onClick.AddListener(SubmitMessage);
            _chatInputField.onSubmit.AddListener(_ => SubmitMessage());
        }

        public void DisplayChatMessage(string message)
        {
            _chatContentText.text += $"\n{message}";
            Canvas.ForceUpdateCanvases();
            _scrollRect.verticalNormalizedPosition = 0f;
        }

        public void ClearChatInput()
        {
            _chatInputField.text = string.Empty;
            _chatInputField.ActivateInputField();
        }
        
        private void SubmitMessage()
        {
            string originalText = _chatInputField.text;
            if (string.IsNullOrWhiteSpace(originalText)) return;
            
            (MessageChannel channel, string cleanText) = ProcessInput(originalText);
        
            _messageChannel = channel;
            
            if (!string.IsNullOrEmpty(cleanText)) OnMessageSubmit?.Invoke(cleanText, _messageChannel);

            ClearChatInput();
        }

        private (MessageChannel, string) ProcessInput(string input)
        {
            if (input.StartsWith("/team", StringComparison.OrdinalIgnoreCase))
                return (MessageChannel.Team, input.Substring(5).Trim());

            if (input.StartsWith("/all", StringComparison.OrdinalIgnoreCase))
                return (MessageChannel.All, input.Substring(4).Trim());
            
            return (_messageChannel, input.Trim());
        }
    }
}