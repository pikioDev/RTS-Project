using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xenocode.Features.Unit.Scripts.Delivery;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "FactoryConfig", menuName = "Xenocode/Factory Config", order = 1)]
    public class FactoryConfig : ScriptableObject
    {
        [Header("Pool Settings")]
        [SerializeField] private List<UnitProfile> unitProfiles;
        
        [SerializeField] private int _initialPoolSize = 10;

        public NetworkUnit GetPrefab(UnitType type)
        {
           return unitProfiles.FirstOrDefault(p => p.UnitType == type)?.Prefab.GetComponent<NetworkUnit>();
        }

        public int GetInitialSize() => _initialPoolSize;
    }
}