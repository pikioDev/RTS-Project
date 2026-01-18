using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using Xenocode.Features.BuildCreator.Scripts.Domain.Model;
using Xenocode.Features.BuildCreator.Scripts.Domain.Provider;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.BuildCreator.Scripts.Delivery
{
    public class BuildingCreatorView : NetworkBehaviour, IBuildingCreatorView
    {
        public NetworkVariable<Team> _playerTeam { get; } = new();
        public event Action OnBuildingMade;
        public event Action<Vector3> OnMouseMoved;
        public event Action<Vector3> OnTryBuild;
        public event Action<int> OnSelectBuilding;

        [SerializeField] private BuildingsDatabaseSO buildingsDatabase;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private LayerMask _groundLayerMask;
        [SerializeField] private Material _validGhostMat;
        [SerializeField] private Material _invalidGhostMat;
        [SerializeField] private GameObject _currentGhost;
        
        private Camera _mainCamera;
        private bool _initialized;
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                _mainCamera = Camera.main;
                BuildingCreatorProvider.Present(this);
                _playerTeam.OnValueChanged += OnTeamChanged;
                OnTeamChanged(_playerTeam.Value, _playerTeam.Value);
            }
        }
        
        private void OnTeamChanged(Team previousValue, Team newValue)
        {
            if (!IsOwner || _initialized) return;
            _initialized = true;
        }

        private void Update()
        {
            if (!IsOwner || !_initialized) return;
            
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000f, _groundLayerMask))
            {
                OnMouseMoved?.Invoke(hit.point);
                
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;
                    OnTryBuild?.Invoke(hit.point);
                }
            }
        }
        
        public void UpdateGhost(Vector3 position, bool isValid)
        {
            _currentGhost.SetActive(true);
            _currentGhost.transform.position = position;
            _renderer.material = isValid ? _validGhostMat : _invalidGhostMat;
        }   
        
        public void TurnOffGhost()
        {
            _currentGhost.SetActive(false);
        }
        
        public Team GetTeam() => _playerTeam.Value;
        
        public void SetTeam(Team team)
        {
            if (!IsServer) return;
            _playerTeam.Value = team;
        }
        
        public void RequestBuildOnServer(Vector3 position, BuildingType buildingType)
        {
            RequestBuildServerRpc(position, (int)buildingType);
            OnBuildingMade?.Invoke();
        }
        
        [ServerRpc]
        private void RequestBuildServerRpc(Vector3 position, int buildingIndex, ServerRpcParams rpcParams = default)
        {
            var clientId = rpcParams.Receive.SenderClientId;
            var buildingType = (BuildingType)buildingIndex;
            BuildingProfileSo profile = buildingsDatabase.GetProfileByType(buildingType);
            
            var playerSupplies = MatchProvider.GetMatchService().GetPlayerRepository().GetPlayer(clientId);
            
            if (playerSupplies.TrySpendGoldServer(profile.GetCost()))
            {
                var buildingInstance = Instantiate(profile.GetBuildingPrefab(), position, Quaternion.identity);
        
                if (buildingInstance.TryGetComponent<IBuildingNetView>(out var buildingView))
                {
                    buildingView.SetTeam(_playerTeam.Value);
                    buildingView.SetBaseMapSprite();
                }
                buildingInstance.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            }
        }
    }
}