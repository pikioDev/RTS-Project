using System;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.UserSupplies.Scripts.Domain.Model;
using Xenocode.Features.UserSupplies.Scripts.Domain.Provider;

namespace Xenocode.Features.UserSupplies.Scripts.Delivery
{
    public class UserSuppliesNet : NetworkBehaviour, IUserSuppliesNet
    {
        [SerializeField] UserSuppliesView _view;
        
        public NetworkVariable<int> CurrentGold = new();
        
        public event Action<int> OnIncomeNotification;
        public event Action<int> OnGoldValueUpdated;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)    
            { 
                _view.gameObject.SetActive(true);
                UserSuppliesProvider.Present(_view, this);
            }
            else 
            {
                _view.gameObject.SetActive(false);
            }
        }
        
        public void AddGold(int amount)
        {
            if (!IsServer) return;
            var newValue = amount;
            CurrentGold.Value += newValue;
        }

        public bool TrySpendGoldServer(int amount)
        {
            if (!IsServer) return false;

            if (CurrentGold.Value >= amount)
            {
                CurrentGold.Value -= amount;
                return true; 
            }

            return false;
        }

        [ClientRpc] public void NotifyKillRewardClientRpc(int reward)
        {
            if (!IsOwner) return;
            _view.NotifyKillReward(reward);
        }
    }
}