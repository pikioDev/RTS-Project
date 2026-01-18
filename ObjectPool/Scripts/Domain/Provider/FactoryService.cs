using System;
using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Model;
using Xenocode.Features.ObjectPool.Scripts.Domain.Services;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Provider
{
    public static class FactoryService
    {
        // We now use a Dictionary to hold a distinct pool for each UnitType
        private static Dictionary<UnitType, IUnitPoolService<INetworkUnit>> _pools;
        private static bool _isInitialized = false;

        static FactoryService()
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (_isInitialized) return;

            _pools = new Dictionary<UnitType, IUnitPoolService<INetworkUnit>>();
            
            // Load config once
            var config = Resources.Load<FactoryConfig>("FactoryConfig");
            var initialSize = config.GetInitialSize();

            // Iterate over every UnitType defined in the Enum
            foreach (UnitType type in Enum.GetValues(typeof(UnitType)))
            {
                var prefab = config.GetPrefab(type);

                if (prefab == null)
                {
                    Debug.LogWarning($"[FactoryService] No prefab found for UnitType: {type}");
                    continue;
                }

                // Create a specific factory method for this specific prefab
                Func<INetworkUnit> specificFactoryMethod = () => 
                    UnityEngine.Object.Instantiate(prefab).GetComponent<INetworkUnit>();

                var pool = new UnitPoolService<INetworkUnit>(
                    specificFactoryMethod,
                    OnUnitSpawned,
                    OnUnitDespawned,
                    initialSize
                );

                _pools.Add(type, pool);
            }

            _isInitialized = true;
        }

        public static INetworkUnit GetObjectFromPool(
            Transform spawnPoint, 
            ulong ownerClientId, 
            UnitType unitType,
            int teamIndex)
        {
            if (!_pools.TryGetValue(unitType, out var pool))
            {
                Debug.LogError($"[FactoryService] No pool exists for UnitType: {unitType}");
                return null;
            }

            return pool.GetObject(spawnPoint, ownerClientId, unitType, teamIndex);
        }

        public static void ReturnObjectToPool(INetworkUnit networkUnitView)
        {
            if (networkUnitView == null) return;

            // We determine which pool to return to based on the Unit's type
            // Assuming INetworkUnit exposes the type via a property or method
            var type = networkUnitView.GetUnitType(); 

            if (_pools.TryGetValue(type, out var pool))
            {
                pool.ReturnObjectToPool(networkUnitView);
            }
            else
            {
                Debug.LogError($"[FactoryService] Tried to return unit of type {type} but no pool was found.");
                // Fallback: Destroy it manually if pooling fails to avoid floating objects
                UnityEngine.Object.Destroy((networkUnitView as MonoBehaviour)?.gameObject);
            }
        }
        
        private static void OnUnitSpawned(INetworkUnit network, Transform spawnPoint, ulong ownerId, UnitType unitType, int teamIndex)
        {
            var networkObject = network.GetNetworkObject();
            var view = network.GetView();
            var newGuid = Guid.NewGuid();

            // Setup Data
            network.SetGuid(newGuid);
            network.SetUnitType(unitType);
            network.SetTeam(teamIndex);

            // Setup Physics/Transform
            view.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            view.gameObject.SetActive(true);

            // Multiplayer Services 1.1.4 Logic
            // We check IsSpawned to prevent errors if the object was just instantiated (not yet spawned) 
            // vs pulled from pool (already spawned but hidden).
            if (networkObject != null && !networkObject.IsSpawned)
            {
                networkObject.SpawnWithOwnership(ownerId);
            }
            // Note: If object is pooled, it keeps its NetworkId. 
            // Ensure your Netcode logic handles re-parenting or ownership changes if needed here.
        }

        private static void OnUnitDespawned(INetworkUnit networkUnit)
        {
            var networkObject = networkUnit.GetNetworkObject();
            
            // For pooled network objects, we usually prefer to Despawn them from the network
            // so they disappear for clients, but we keep the C# instance alive in the pool.
            if (networkObject != null && networkObject.IsSpawned)
            {
                networkObject.Despawn(false); // false = don't destroy the gameObject
            }

            ((MonoBehaviour)networkUnit).gameObject.SetActive(false);
        }
    }
}