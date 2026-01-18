using System;
using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;
using Xenocode.Features.BehaviorTree.Scripts.Domain.Model;
using Xenocode.Features.FSM.Domain.Model;
using Xenocode.Features.Match.Scripts.Delivery;
using Xenocode.Features.Match.Scripts.Domain.Services;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Services;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Services;
using Xenocode.Features.Waypoints.Scripts.Domain.Services;

namespace Xenocode.Features.Unit.Scripts.Domain.Presentation
{
    public class UnitPresenter
    {
        private readonly IUnitView _view;
        private readonly INetworkUnit _netUnit;
        private readonly ulong _ownerClientId;
        private readonly WaypointsService _waypointsService;
        private readonly MatchService _matchService;
        private readonly TeamService _teamService;
        private readonly IUnitService _unitService;
        private readonly Guid _guid;
        private readonly UnitStateMachine _unitStateMachine;
        private readonly Opsive.BehaviorDesigner.Runtime.BehaviorTree _behaviorTree;
        private readonly ISuppliesService _suppliesService;
        private readonly IAudioEvents _audioService;


        public UnitPresenter(INetworkUnit netUnit, ulong ownerClientId, WaypointsService waypointsService,
            MatchService matchService, TeamService teamService, IUnitService unitService, Guid guid,
            UnitStateMachine unitStateMachine, Opsive.BehaviorDesigner.Runtime.BehaviorTree behaviorTree,
            UnitEvents unitEvents, ISuppliesService suppliesService, IAudioEvents audioService)
        {
            _netUnit = netUnit;
            _view = netUnit.GetView();
            _ownerClientId = ownerClientId;
            _waypointsService = waypointsService;
            _matchService = matchService;
            _teamService = teamService;
            _unitService = unitService;
            _guid = guid;
            _unitStateMachine = unitStateMachine;
            _behaviorTree = behaviorTree;
            _suppliesService = suppliesService;
            _audioService = audioService;
            SubscribeToUnitEvents(unitEvents);
            SubscribeToViewEvents();
        }

        private void SubscribeToUnitEvents(UnitEvents unitEvents)
        {
            unitEvents.OnDamageTaken.AddListener(HandleDamageTaken);
            unitEvents.OnAttack.AddListener(HandleAttacking);
            unitEvents.OnTypeChange.AddListener(HandleTypeUpdate);
            unitEvents.OnReachTarget.AddListener(HandleReachTarget);
        }
        
        private void HandleReachTarget(bool onRange)
        {
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.OnRange, onRange);
        }
        private void HandleTypeUpdate(UnitType newType)
        {
            var profile = _unitService.GetUnitProfileByUnitType(newType);
            profile.SetUnitType(newType);
        }

        private void SubscribeToViewEvents()
        {
            _view.OnAppear().AddListener(HandleOnAppear);
            _view.OnViewDestroyed().AddListener(HandleOnDestroy);
        }


        private void HandleAttacking(Vector3 position, float lifeTime)
        {
            _netUnit.PerformAttackClient(position, lifeTime, (int)GetUnitProfile().StrikeData.StrikeType);
            _audioService.PlaySound(GetUnitProfile().AudioProfile.name, (int)AudioClipType.Combat, _netUnit.GetPosition());
        }

        private void HandleOnDie()
        {
            _audioService.PlaySound(GetUnitProfile().AudioProfile.name, (int)AudioClipType.Death, _netUnit.GetPosition());
            _netUnit.DisplayDeathClient();
        }
        
        private void HandleOnDestroy()
        {
            _behaviorTree.GetVariable(BehaviourTreeVariables.UnitState).
                OnValueChange -= HandleBehaviourTreeStateChanged;
            
            _unitStateMachine.StopUpdateState();
            _behaviorTree.enabled = false;
            _unitService.RemoveUnit(_guid);
            _netUnit.ReturnToPool();
        }
        
        private void HandleKillReward(ulong attackerId)
        {
            _suppliesService.HandleIncomeByKill(attackerId, GetUnitProfile().GoldReward);
        }
        
        private void HandleOnMove()
        {
            _view.EnableWaypointsBehaviour();
            _unitStateMachine.ChangeState(new MoveState());
        }
        
        private void HandleOnWalkingTarget()
        {
            _view.DisableMovement();
            _unitStateMachine.SetTarget((Transform) _behaviorTree.GetVariable(BehaviourTreeVariables.Target).GetValue());
            _view.SetDestination((Transform) _behaviorTree.GetVariable(BehaviourTreeVariables.Target ).GetValue());
            _unitStateMachine.ChangeState(new WalkingTargetState());
        }
        
        private void HandleOnAttack()
        {
            _view.DisableMovement();
            _unitStateMachine.SetTarget((Transform) _behaviorTree.GetVariable(BehaviourTreeVariables.Target).GetValue());
            _unitStateMachine.ChangeState(new AttackState());
        }

        private void HandleDamageTaken(int damageAmount, ulong attackerId)
        {
            UpdateHealth(damageAmount, attackerId);
        }

        private void UpdateHealth(int damageAmount, ulong attackerId)
        {
            if (!_unitService.IsUnitRegistered(_guid)) return;
            
            var profile = GetUnitProfile();
            if (_unitService.GetUnitCurrentHealth(_guid) <= 0) return;
            
            _unitService.UpdateUnitHealth(damageAmount, _guid);
            var currentHealth = _unitService.GetUnitCurrentHealth(_guid);
            
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.CurrentHealth, currentHealth);
            _netUnit.SetNetworkHealth(currentHealth);
            _view.UpdateHealthVisual(currentHealth, profile.MaxHealth);
            
            if (currentHealth <= 0)
            {
                HandleKillReward(attackerId);
                HandleOnDie();
            }
        }
        
        private void HandleOnAppear(UnitType type)
        {
            var team = _matchService.GetTeam(_ownerClientId);
            var profile = _unitService.GetUnitProfileByUnitType(type);
            var waypoints = _waypointsService.GetWaypoints(team);
            var finalWaypoints = _waypointsService.GetFinalWaypoint(team);
            _unitService.AddToUnitsRepository(_guid, profile);
            _netUnit.SetNetworkHealth(profile.MaxHealth);
            _view.UpdateHealthVisual(profile.MaxHealth, profile.MaxHealth);
            _view.SetupWaypointBehaviour(waypoints, finalWaypoints);
            _view.SetUnitMovementSpeed(profile.MoveSpeed);
            _view.SetTeamColor(_teamService.GetTeamColor(team));
            _netUnit.SetTeamColorClientRpc(_teamService.GetTeamColor(team));
            _view.SetTeam(team);
            _view.SetAgentStopDistance(profile.StrikeData.Range);
            _audioService.PlaySound(profile.AudioProfile.name, (int)AudioClipType.Spawn, _netUnit.GetPosition());
            SetupBehaviourTree(profile.MaxHealth);
        }

        private void SetupBehaviourTree(int maxHealth)
        {
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.CurrentHealth,maxHealth);
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.UnitState, (int) UnitBehaviorTreeState.Idle);
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.AttackRange, GetUnitProfile().StrikeData.Range);
            _behaviorTree.GetVariable(BehaviourTreeVariables.UnitState).OnValueChange += HandleBehaviourTreeStateChanged;
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.Target, (Transform)null);
            _behaviorTree.SetVariableValue(BehaviourTreeVariables.OnRange, false);
            _behaviorTree.enabled = true;
        }
        
        private void HandleBehaviourTreeStateChanged()
        {
            var state = (UnitBehaviorTreeState) _behaviorTree.GetVariable(BehaviourTreeVariables.UnitState).GetValue();
            switch (state)
            {
                case UnitBehaviorTreeState.Idle:
                    _netUnit.UpdateMovementSpeedClientRpc();
                    break;
                case UnitBehaviorTreeState.Walking_Waypoints:
                    HandleOnMove();
                    _netUnit.UpdateMovementSpeedClientRpc(1);
                    break;
                case UnitBehaviorTreeState.Attack:
                    HandleOnAttack();
                    _netUnit.UpdateMovementSpeedClientRpc();
                    break;
                case UnitBehaviorTreeState.Die:
                    HandleOnDie();
                    break;
                case UnitBehaviorTreeState.Walking_Target:
                    HandleOnWalkingTarget();
                    _netUnit.UpdateMovementSpeedClientRpc(0.1f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private int GetUnitMaxHealth() => _unitService.GetUnitData(_guid).Profile.MaxHealth;
        private UnitProfile GetUnitProfile() 
        {
            if (!_unitService.IsUnitRegistered(_guid)) return null;
            return _unitService.GetUnitData(_guid).Profile;
        }
    }
}