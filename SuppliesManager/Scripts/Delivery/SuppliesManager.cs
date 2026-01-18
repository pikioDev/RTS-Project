using System;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Model;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Provider;
using Xenocode.Features.SuppliesManager.Settings;

namespace Xenocode.Features.SuppliesManager.Scripts.Delivery
{
    public class SuppliesManager : NetworkBehaviour, ISuppliesManager
    {
        [SerializeField] private MatchSettingsSO settings;
        
        public event Action<float> OnServerTick;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                SuppliesManagerProvider.Present(this, settings);
            }
        }
        private void Update()
        {
            if (IsServer)
            {
                OnServerTick?.Invoke(Time.deltaTime);
            }
        }
    }
}
