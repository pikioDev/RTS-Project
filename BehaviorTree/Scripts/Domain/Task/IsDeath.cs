using Opsive.BehaviorDesigner.Runtime.Tasks;
using Opsive.BehaviorDesigner.Runtime.Tasks.Conditionals;
using Opsive.GraphDesigner.Runtime.Variables;

namespace Xenocode.Features.BehaviorTree.Scripts.Domain.Task
{
    public class IsDeath : Conditional, IConditionalReevaluation
    {
        public SharedVariable<int> CurrentHealth;

        public override TaskStatus OnUpdate()
        {
            return CurrentHealth.Value <= 0 ? TaskStatus.Success : TaskStatus.Failure;
        }
        
        public TaskStatus OnReevaluateUpdate()
        {
            return OnUpdate();
        }
    }
    
}