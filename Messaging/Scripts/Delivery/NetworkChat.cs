using System;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Messaging.Scripts.Domain.Model;
using Xenocode.Features.Messaging.Scripts.Domain.Provider;

namespace Xenocode.Features.Messaging.Scripts.Delivery
{
    public class NetworkChat : NetworkBehaviour, INetworkChat
    {
        [SerializeField] ChatView _view;
        public event Action<MessageData> OnMessageReceived;
        public IChatView GetView() => _view;
        
        private void Awake()
        {
            MessagingProvider.Present(this); 
        }
        
        public void SendRequest(string content, MessageChannel channel)
        {
            SubmitMessageServerRpc(content, (int)channel);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SubmitMessageServerRpc(string content, int channel, ServerRpcParams rpcParams = default)
        {
            string senderName = $"Player {rpcParams.Receive.SenderClientId}";
            ReceiveMessageClientRpc(content, senderName, channel);
        }
        
        [ClientRpc]
        private void ReceiveMessageClientRpc(string content, string senderName, int channel)
        {
            var data = new MessageData(senderName, content, (MessageChannel)channel);
            OnMessageReceived?.Invoke(data);
        }
    }
}