using System;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Domain.Providers;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Delivery
{
    public class BuildingNetView : NetworkBehaviour, IBuildingNetView
    {
        [SerializeField] private BuildingView _view; 
        [SerializeField] private NetworkObject _networkObject;
        
        private readonly NetworkVariable<int> _teamNet = new(0); 
        private readonly NetworkVariable<float> _timerRemainingNet = new(0f);
        private readonly NetworkVariable<int> _hitsPointsNet = new(0);
        
        public event Action<float> OnTimerRemainingChanged;
        public event Action OnServerUpdateTimer;
        public event Action<int> OnHitsPointsChanged;
        
        public ulong GetOwnerClientId() => _networkObject.OwnerClientId;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            _timerRemainingNet.OnValueChanged += HandleNetTimerValueChanged;
            _hitsPointsNet.OnValueChanged += HandleHitsPointsChanged;
            _view.SetNetView(this);
            
            if (IsServer)
            {
                BuildingProvider.Present(_view, this, _view.GetBuildingProfile()); 
            }
            if(IsOwner)
                BuildingProvider.SelectBuildOnSpawn(this);
        }

        private void HandleNetTimerValueChanged(float previousValue, float newValue)
        {
            OnTimerRemainingChanged?.Invoke(newValue);
        }

        private void HandleHitsPointsChanged(int previousValue, int newValue)
        {
            OnHitsPointsChanged?.Invoke(newValue);
        }
        
        public void SetTimerRemaining(float time)
        {
            if (IsServer)
            {
                _timerRemainingNet.Value = time;
            }
        }

        public float GetTimerRemaining() => _timerRemainingNet.Value;
        public int GetHitPoints() => _hitsPointsNet.Value;
        public void SetBaseMapSprite()
        {
            if (!IsServer) return;
            SetBuildingTextureClientRpc();
        }

        public bool IsAlly() => OwnerClientId == NetworkManager.Singleton.LocalClientId;
        public Vector3 GetPosition() => transform.position;

        [ClientRpc] private void SetBuildingTextureClientRpc()
        {
            _view.ChangeTexture();
        }

        public void SetHitsPointsNet(int getHitPoints)
        {
            _hitsPointsNet.Value = getHitPoints;
            OnHitsPointsChanged?.Invoke(getHitPoints);
        }
        

        void Update()
        {
            if (IsServer) 
                OnServerUpdateTimer?.Invoke(); 
        }

        public int GetTeamIndex() => _teamNet.Value;

        public void SetTeam(Team playerTeam)
        {
            if(IsServer)
            {
                _teamNet.Value = (int)playerTeam; 
            }
        }

        public BuildingProfileSo GetBuildingProfile() => _view.GetBuildingProfile();
        
        
#if UNITY_EDITOR
        /// <summary>
        /// Spawns a single unit immediately. This method is for editor use only.
        /// It requires the application to be in Play Mode and must be called on the server/host.
        /// </summary>
        public void Editor_SpawnUnit()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Spawning is only available in Play Mode.");
                return;
            }

            // La verificación de autoridad es necesaria
            if (!IsServer)
            {
                Debug.LogWarning("Spawning can only be initiated on the server/host.");
                return;
            }

            Debug.Log("Spawning unit via inspector button...", this);
            
            // 🎯 Llama a la lógica de spawn del Dominio/View (que ejecuta el FactoryService)
            _view.SpawnUnit(_teamNet.Value);
        }
#endif
    }
}