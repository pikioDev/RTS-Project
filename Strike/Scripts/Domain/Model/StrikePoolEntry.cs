using System;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Model;

namespace Xenocode.Features.Strike.Scripts.Domain.Model
{
    [Serializable] 
    public class StrikePoolEntry
    {
        [field: SerializeField] public StrikeType Type { get; private set; }
        [field: SerializeField] public Strike Prefab { get; private set; } 
        [field: SerializeField] public int InitialSize { get; private set; } = 10;
        
        public IStrike GetPrefab() => Prefab;
        public StrikeType GetStrikeType() => Type;
        public int GetInitialSize() => InitialSize;
    }
}