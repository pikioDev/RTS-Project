using System;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.WorldSelector.Scripts.Domain.Model;
using Xenocode.Features.WorldSelector.Scripts.Domain.Provider;

namespace Xenocode.Features.WorldSelector.Scripts.Delivery
{
    public class WorldSelectorView : MonoBehaviour, IWorldSelectorView
    {
        public event Action<INetworkUnit> OnUnitClicked;
        public event Action<IBuildingNetView> OnBuildingClicked; 
        public event Action<Vector3> OnTerrainClicked;
        public event Action OnCancelClick;
        [SerializeField] private LayerMask _unitLayerMask;
        [SerializeField] private LayerMask _buildingLayerMask;
        [SerializeField] private LayerMask _terrainLayerMask;
        private Camera _mainCamera;
        private LayerMask _combinedLayerMask;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            _combinedLayerMask = _unitLayerMask | _buildingLayerMask | _terrainLayerMask;
        }

        void Start()
        {
            WorldSelectorProvider.Present(this);
        }
        private void Update()
        {
            CheckForClick(); 
        }

        private void CheckForClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _combinedLayerMask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    
                    if (hitObject.TryGetComponent<INetworkUnit>(out INetworkUnit unit))
                    {
                        OnUnitClicked?.Invoke(unit);
                    }
                    else if (hitObject.TryGetComponent<IBuildingNetView>(out IBuildingNetView building))
                    {
                        OnBuildingClicked?.Invoke(building);
                    }
                    else if ((1 << hitObject.layer & _terrainLayerMask) != 0)
                    {
                        OnTerrainClicked?.Invoke(hit.point);
                    }
                }
            }

            if (Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancelClick?.Invoke();
            }
        }
    }
}