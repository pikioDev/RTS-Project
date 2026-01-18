using UnityEngine;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Model
{
    public interface IUnitPoolService<TIUnitNetView>
    {
        TIUnitNetView GetObject(Transform spawnPoint, ulong ownerClientId, UnitType unitType, int teamIndex);
        void ReturnObjectToPool(TIUnitNetView obj);
    }
}