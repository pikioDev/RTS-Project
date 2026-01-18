using UnityEngine.AI;
using Xenocode.Features.AttackStrategy.Scripts.Domain.Model;
using Xenocode.Features.FSM.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.FSM.Domain.Providers
{
    public static class FSMProvider
    {
        public static UnitStateMachine GetUnitStateMachine(UnitProfile profile, NavMeshAgent agent, IAttackStrategy attackStrategy) => new(profile,agent, attackStrategy);
    }
}