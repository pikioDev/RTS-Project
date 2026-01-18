using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.Strike.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "StrikeFactoryConfig", menuName = "Xenocode/Strike Factory Config", order = 1)]
    public class StrikeFactoryConfig : ScriptableObject
    {
        [Header("Pool Settings")]
        [SerializeField] private List<StrikePoolEntry> _poolEntries = new List<StrikePoolEntry>();

        public IEnumerable<StrikePoolEntry> GetAllEntries()
        {
            return _poolEntries;
        }
    }
}
