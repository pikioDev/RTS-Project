using UnityEngine;

namespace Xenocode.Features.FSM.Domain.Model
{
    public class DieState : State
    {
        private float _destroyTimer = 2f; 

        public override void EnterState(UnitStateMachine sm)
        {
          //  sm.NetworkState.Value = UnitState.Dying; 
         //   sm.GetComponent<Collider>().enabled = false;
            sm.Agent.enabled = false;
        }

        public override void UpdateState(UnitStateMachine sm)
        {
            _destroyTimer -= Time.deltaTime;
            if (_destroyTimer <= 0f)
            {
         //       sm.GetComponent<NetworkObject>().Despawn();
            }
        }

        public override void ExitState(UnitStateMachine sm) { }
    }
}