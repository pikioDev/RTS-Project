using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class FindTargets : Conditional, IConditionalReevaluation 
    {
        public SharedVariable<Transform> Target;
        
        public override TaskStatus OnUpdate()
        {
            if(!Target.Value) return TaskStatus.Failure;
            return TaskStatus.Success;
        }
        
        public TaskStatus OnReevaluateUpdate()
        {
            return OnUpdate();
        }
    }
}