using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Xenocode.Features.AttackStrategy.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.FSM.Domain.Model
{
    public class UnitStateMachine 
    {
        public NavMeshAgent Agent { get; private set; }
        private State _currentState;
        private UnitProfile _profile;
        private CancellationTokenSource _cancellationTokenSource;
        public Transform CurrentTarget{ get; private set; }
        public IAttackStrategy AttackStrategy { get; private set; } 

        public UnitStateMachine(UnitProfile profile, NavMeshAgent agent, IAttackStrategy attackStrategy)
        {
            _profile = profile;
            Agent = agent;
            AttackStrategy = attackStrategy;
        }
        public UnitProfile GetUnitProfile() => _profile;
        
        public void ChangeState(State newState)
        {
            if (_currentState == newState) return;
            StopUpdateState();
            _currentState?.ExitState(this);
            
            _currentState = newState;
            _currentState.EnterState(this);
            StartUpdateState();
        }

        private void StartUpdateState()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        
            UpdateStateLoop(_cancellationTokenSource.Token).Forget();
        }
        
        private async UniTaskVoid UpdateStateLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
                _currentState.UpdateState(this);
            }
        }

        public void StopUpdateState()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void SetTarget(Transform transform)
        {
            CurrentTarget =  transform;
        }
    }
}
