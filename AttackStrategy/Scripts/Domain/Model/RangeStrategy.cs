using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Match.Scripts.Delivery;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.AttackStrategy.Scripts.Domain.Model
{
    public class RangeStrategy : IAttackStrategy
    {
        public async UniTask Execute(Transform attacker, Transform target, UnitProfile profile)
        {
            if (!attacker || !target) return;
            
            ulong attackerId = 0;
            if (attacker.TryGetComponent<NetworkObject>(out var netObj))
            {
                attackerId = netObj.OwnerClientId;
            }
            

            if (attacker.TryGetComponent<UnitEvents>(out var unitEvents))
            {
                unitEvents.OnAttack.Invoke(target.position, profile.StrikeData.Lifetime);
            }
            
            try 
            {
                await UniTask.Delay(TimeSpan.FromSeconds(profile.StrikeData.Lifetime), 
                    cancellationToken: attacker.GetCancellationTokenOnDestroy());
            }
            catch (OperationCanceledException) { return; }

            if (target && target.TryGetComponent<UnitEvents>(out var targetEvents))
            {
                targetEvents.OnDamageTaken.Invoke(profile.StrikeData.Damage, attackerId);
            }
        }
    }
}
    
