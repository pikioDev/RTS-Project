using UnityEngine;
using Xenocode.Features.Strike.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Model
{
    public interface IStrikePoolService<IStrike>
    {
        IStrike GetObject(Transform spawnPoint, StrikeType strikeType);
        
        void ReturnObject(IStrike obj);
    }
}