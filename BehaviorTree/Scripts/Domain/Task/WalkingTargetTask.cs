using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;
using Xenocode.Features.BehaviorTree.Scripts.Domain.Model;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class WalkingTargetTask : Conditional
    {
        public SharedVariable<Transform> Target;
        public SharedVariable<bool> OnRange;
        public SharedVariable<int> UnitState;

        public override void OnStart()
        {
            UnitState.Value = (int)UnitBehaviorTreeState.Walking_Target;
        }

        public override TaskStatus OnUpdate()
        {
            if (!Target.Value) return TaskStatus.Failure;
            transform.LookAt(Target.Value);
            return TaskStatus.Running;
        }
    }
}