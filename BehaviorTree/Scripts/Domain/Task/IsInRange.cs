using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class IsInRange : Conditional
    {
        public SharedVariable<Transform> target;
        public SharedVariable<float> attackRange;

        public override TaskStatus OnUpdate()
        {
            if (!target.Value || !target.Value.gameObject.activeInHierarchy)
            {
                target.Value = null; 
                return TaskStatus.Failure;
            }

            float distance = Vector3.Distance(transform.position, target.Value.position);

            if (distance <= attackRange.Value + 0.2f)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}