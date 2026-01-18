namespace Xenocode.Features.FSM.Domain.Model
{
    public abstract class State
    {
        public abstract void EnterState(UnitStateMachine stateMachine);
        
        public abstract void UpdateState(UnitStateMachine stateMachine);
        
        public abstract void ExitState(UnitStateMachine stateMachine);
    }
}