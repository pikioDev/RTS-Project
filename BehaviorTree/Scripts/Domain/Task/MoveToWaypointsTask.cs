using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using Xenocode.Features.BehaviorTree.Scripts.Domain.Model;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class MoveToWaypointsTask : Action
    {
        public SharedVariable<int> UnitState;
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

        public override void OnStart()
        {
            UnitState.Value = (int)UnitBehaviorTreeState.Walking_Waypoints;
        }
    }
}