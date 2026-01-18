using UnityEngine;
using Xenocode.Features.Match.Scripts.Delivery;

namespace Xenocode.Features.FSM.Domain.Model
{
    public class AttackState : State
    {
        private float _attackTimer;

        public override void EnterState(UnitStateMachine sm)
        {
            PerformAttack(sm);
        }
        
        public override void UpdateState(UnitStateMachine sm)
        {
            if(!HasTarget(sm.CurrentTarget)) return;
            AdvanceCooldown();
            if (!IsCooldownCompleted(sm)) return;
            ResetCooldown();
            PerformAttack(sm);
        }
        
        private void PerformAttack(UnitStateMachine sm)
        {
            sm.AttackStrategy?.Execute(
                sm.Agent.transform,
                sm.CurrentTarget,         
                sm.GetUnitProfile()       
            );
            _attackTimer = 0f;
        }

        private bool HasTarget(Transform smCurrentTarget) => smCurrentTarget;
        
        private void AdvanceCooldown()
        {
            _attackTimer += Time.deltaTime;
        }

        private void ResetCooldown()
        {
            _attackTimer = 0f;
        }

        private bool IsCooldownCompleted(UnitStateMachine sm) => 
            _attackTimer >= sm.GetUnitProfile().StrikeData.Cooldown;

        public override void ExitState(UnitStateMachine sm)
        {
            if (sm.Agent.TryGetComponent<UnitEvents>(out var unitEvents))
            {
                unitEvents.OnReachTarget.Invoke(false);
            }
        }
    }
}