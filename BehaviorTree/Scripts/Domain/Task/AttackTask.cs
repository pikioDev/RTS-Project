using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;
using Xenocode.Features.BehaviorTree.Scripts.Domain.Model;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class AttackTask : Action
    {
        public SharedVariable<Transform> target;
        public SharedVariable<int> UnitState;
        
        public override void OnStart()
        {
            UnitState.Value = (int)UnitBehaviorTreeState.Attack;
        }

        public override TaskStatus OnUpdate()
        {
            if (!target.Value || !target.Value.gameObject.activeInHierarchy)
            {
                target.Value = null;
                return TaskStatus.Failure; 
            }
            
            transform.LookAt(target.Value);
            return TaskStatus.Running;
        }
    }
}