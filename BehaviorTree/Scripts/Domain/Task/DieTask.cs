using Opsive.BehaviorDesigner.Runtime.Tasks.Actions;
using Opsive.GraphDesigner.Runtime.Variables;
using UnityEngine;
using Xenocode.Features.BehaviorTree.Scripts.Domain.Model;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class DieTask : Action
    {
        public SharedVariable<int> UnitState;
        public SharedVariable<Transform> Target;
        public override void OnStart()
        {
            UnitState.Value = (int)UnitBehaviorTreeState.Die;
            Target.Value = null;
        }
    }
}