using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Model
{
    public interface IUnitView
    {
        UnityEvent<UnitType> OnAppear();
        
        UnityEvent OnViewDestroyed();
        
        void SetupWaypointBehaviour(List<Vector3> waypoints, Vector3 finalWaypoints);
        
        void SetTeamColor(Color teamColor);
        
        void SetUnitMovementSpeed(float unitMovementSpeed);
        
        void SetTeam(Team team);
        
        void DisableMovement();
        
        void SetDestination(Transform destination);
        
        void EnableWaypointsBehaviour();
        
        void UpdateHealthVisual(int profileMaxHealth, int maxHealth);
        
        void SetAgentStopDistance(float profileAttackRange);
        void UpdateAnimatorSpeed(NetworkAnimator animator, float speed);
    }
}