using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Xenocode.Features.Match.Scripts.Delivery;
using Xenocode.Features.Teams.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.Waypoints.Scripts.Delivery;

namespace Xenocode.Features.Unit.Scripts.Delivery
{
    public class UnitView : MonoBehaviour, IUnitView
    {
        [Header("Components")]
        [SerializeField] private WaypointMovementBehaviour _waypointMovementBehaviour;
        [SerializeField] private MeshRenderer _teamMeshRenderer;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private HealthBar.Scripts.Delivery.HealthBar _healthBar;
        [SerializeField] private Opsive.BehaviorDesigner.Runtime.BehaviorTree _behaviorTree;
        [SerializeField] private GameObject _unitDeathPrefab;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private Transform _weaponHolder;
        [SerializeField] private Transform _corpseTransform;
        [SerializeField] private UnitEvents _unitEvents;
        [SerializeField] private GameObject _highLight;
        [SerializeField] private Animator _animator;
        private INetworkUnit _network;
        
        public Team Team { get; private set; }
        public UnityEvent<UnitType> OnAppear() => _onAppear;
        private readonly UnityEvent<UnitType> _onAppear = new();
        public UnityEvent OnViewDestroyed() => _onDestroyed;
        private readonly UnityEvent _onDestroyed = new();
        
        public void TriggerOnAppear(UnitType type)
        {
            _onAppear.Invoke(type);
        }
        
        public void SetupWaypointBehaviour(List<Vector3> waypoints, Vector3 finalWaypoints)
        {
            _waypointMovementBehaviour.SetWaypoints(waypoints);
            _waypointMovementBehaviour.SetFinalWaypoint(finalWaypoints);
        }
        
        public void SetTeamColor(Color teamColor)
        {
           // _teamMeshRenderer.material.color = teamColor;
           _healthBar.SetTeamColor(teamColor);
        }
        
        public void SetUnitMovementSpeed(float unitMovementSpeed)
        {
            _agent.speed = unitMovementSpeed;
        }
        
        public void SetTeam(Team team)
        {
            Team = team;
        }
        
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            _agent.enabled = true;
            _agent.Warp(position);
            transform.rotation = rotation;
        }
        
        public void DisableMovement()
        {
            _waypointMovementBehaviour.enabled = false;
            _agent.SetDestination(transform.position);
        }
        
        public void SetDestination(Transform destination)
        {
            if (!destination) return;
            _agent.SetDestination(destination.position);
        }
        
        public void EnableWaypointsBehaviour()
        {
            _waypointMovementBehaviour.enabled = true;
            _waypointMovementBehaviour.Execute();
        }
        
        public void InvokeDestroy()
        {
            _onDestroyed.Invoke();
        }

        public void ShowHighLight()
        {
            _highLight?.SetActive(true);
        }

        public void HideHighLight()
        {
            _highLight?.SetActive(false);
        }
        
        public NavMeshAgent GetAgent() => _agent;

        public UnitEvents GetUnitEvents() => _unitEvents;

        public Opsive.BehaviorDesigner.Runtime.BehaviorTree GetBehaviorTree() => _behaviorTree;

        public Transform GetWeaponHolder() => _weaponHolder;

        public GameObject GetDeathPrefab() => _unitDeathPrefab;

        public void UpdateHealthVisual(int newValue, int getMaxHealth)
        {
            _healthBar.UpdateHealth(newValue, getMaxHealth);
        }

        public void SetAgentStopDistance(float profileAttackRange)
        {
            _agent.stoppingDistance = profileAttackRange - 0.2f;
        }

        public void UpdateAnimatorSpeed(NetworkAnimator networkAnimator, float speed)
        {
            networkAnimator.Animator.SetFloat("Speed", speed);
        }

        public Transform GetCorpsePos() => _corpseTransform;

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}