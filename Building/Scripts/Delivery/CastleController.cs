using System;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.FieldOfView.Scripts;
using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Delivery
{
    public class CastleController : NetworkBehaviour, IFovTarget
    {
        [SerializeField] private int _maxHealth = 1000;
        public NetworkVariable<int> CurrentHealth = new(5000);
        [SerializeField] private NetworkVariable<Team> CastleTeam;
        public int GetMaxHealth() => _maxHealth;
        public int GetCurrentHealth() => CurrentHealth.Value;
        public event Action OnCastleDefeated;
        
        public void Initialize(Team team)
        {
            CastleTeam.Value = team;
            CurrentHealth.Value = _maxHealth;
        }

        void Start()
        {
            MatchProvider.GetMatchService().RegisterCastle(CastleTeam.Value, this);
        }

        public Team GetTeam() => CastleTeam.Value;

        public void UpdateHitPoints(int dmg)
        {
            if (!IsServer) return;
            var newHp = dmg;
            CurrentHealth.Value -= newHp;
        }
    }
}