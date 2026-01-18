using Cysharp.Threading.Tasks;
using UnityEngine;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.AttackStrategy.Scripts.Domain.Model
{
    public interface IAttackStrategy
    {
        UniTask Execute(Transform attacker, Transform target, UnitProfile profile);
    }
}