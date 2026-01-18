using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Xenocode.Features.FieldOfView.Scripts;
using Xenocode.Features.ObjectPool.Scripts.Domain.Provider;
using Xenocode.Features.Strike.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Providers;

namespace Xenocode.Features.Unit.Scripts.Delivery
{
    public class NetworkUnit : NetworkBehaviour, INetworkUnit, IFovTarget
    {
        [SerializeField] private NetworkObject _networkObject; 
        [SerializeField] private NetworkAnimator _networkAnimator; 
        [SerializeField] private UnitView _view; 
        private int _maxHealthCache;
        private Guid _guid;
        
        [Header("Network Variables")]
        private readonly NetworkVariable<int> _currentHealthNet = new(100);
        private readonly NetworkVariable<UnitType> _unitType = new(UnitType.None);
        [SerializeField] private NetworkVariable<Team> _team = new();
        
        public event Action<Guid, int> OnUnitHitPointsChanged;
        
        public Guid GetGuid() => _guid;
        public void SetGuid(Guid guid) => _guid = guid;
        
        public Team GetTeam() => _team.Value;
        public void SetTeam(int teamIndex) => _team.Value = (Team)teamIndex;
        
        public NetworkObject GetNetworkObject() => _networkObject;
        public UnitView GetView() => _view;
        
        public UnitType GetUnitType() => _unitType.Value;
        public void SetUnitType(UnitType unitType) => _unitType.Value = unitType;
        
        public void ShowHighLight() => _view.ShowHighLight();
        public void HideHighLight() => _view.HideHighLight();
        
        public void ReturnToPool() => FactoryService.ReturnObjectToPool(this);
        
        public void DisplayDeathClient()
        {
            DisplayDeathClientRpc();
            _view.InvokeDestroy();
        }

        public int GetCurrentNetworkHealth() => _currentHealthNet.Value;
        
        public void SetNetworkHealth(int health)
        {
            if (IsServer)
            {
                _maxHealthCache = health;
                _currentHealthNet.Value = health;
            }
        }
        
        public void PerformAttackClient(Vector3 target, float lifeTime,  int strikeType)
        {
            PerformVisualAttackClientRpc(target, lifeTime, strikeType);
        }

        public Vector3 GetPosition() => transform.position;
        
        public bool IsAlly() => OwnerClientId == NetworkManager.Singleton.LocalClientId;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            _currentHealthNet.OnValueChanged += HandleNetHealthValueChanged;
            _view.UpdateHealthVisual(_currentHealthNet.Value, _currentHealthNet.Value);
            
            if (IsServer)
                UnitProvider.Present(this, OwnerClientId, GetGuid(), _unitType.Value, 
                    _view.GetAgent(), _view.GetBehaviorTree(), _view.GetUnitEvents());
        
            _view.TriggerOnAppear(_unitType.Value); 
        }
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _currentHealthNet.OnValueChanged -= HandleNetHealthValueChanged;
        }

        private void HandleNetHealthValueChanged(int oldValue, int newValue)
        {
            OnUnitHitPointsChanged?.Invoke(GetGuid(), newValue);
            if (_maxHealthCache <= 0) _maxHealthCache = newValue; 
            _view.UpdateHealthVisual(newValue, _maxHealthCache);
        }
        
        [ClientRpc] public void UpdateMovementSpeedClientRpc(float speed = 0)
        {
            _view.UpdateAnimatorSpeed(_networkAnimator, speed);
        }

        [ClientRpc] public void SetTeamColorClientRpc(Color getTeamColor)
        {
            _view.SetTeamColor(getTeamColor);
        }

        [ClientRpc] void DisplayDeathClientRpc()
        {
            Instantiate(_view.GetDeathPrefab(), _view.GetCorpsePos().position,  _view.GetCorpsePos().rotation);
        }
        
        [ClientRpc] void PerformVisualAttackClientRpc(Vector3 target, float lifeTime, int strikeType)
        {
            var type = (StrikeType)strikeType;
            var spawnPoint = _view.GetWeaponHolder();
            var strike = StrikeFactoryService.GetObj(type, spawnPoint);
            
            if (strike is Strike.Scripts.Domain.Model.Strike attack)
            {
                attack.Execute(spawnPoint, target, lifeTime);
            }
        }
    }
}