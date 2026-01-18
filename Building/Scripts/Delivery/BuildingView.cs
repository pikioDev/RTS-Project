using System.Collections;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.ObjectPool.Scripts.Domain.Provider;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Delivery
{
    public class BuildingView : MonoBehaviour, IBuildingView
    {
        [SerializeField] private BuildingProfileSo _profile;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Renderer _renderer;
        private float spawnFxDuration = 1.0f;
        private float startValue = -2.2f;
        private float endValue = 3.0f;
        private IBuildingNetView _netView;
        private MaterialPropertyBlock _propBlock;


        void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        void OnEnable()
        {
            ChangeTexture();
            StartCoroutine(AppearRoutine());
        }



        IEnumerator AppearRoutine()
        {
            float elapsed = 0;
            while (elapsed < spawnFxDuration)
            {
                elapsed += Time.deltaTime;
                float currentValue = Mathf.Lerp(startValue, endValue, elapsed / spawnFxDuration);
                UpdatePropertyBlockDissolve(currentValue);
                yield return null;
            }
            UpdatePropertyBlockDissolve(endValue);
        }
        
        private void UpdatePropertyBlockDissolve(float value)
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat("_DirectionEdgeWidthScale", value);
            _renderer.SetPropertyBlock(_propBlock);
        }
        
        public void ChangeTexture()
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetTexture("_BaseMap", _profile.GetBuildingTexture());
            _renderer.SetPropertyBlock(_propBlock);
            
        }
        
        public void SetNetView(IBuildingNetView netView)
        {
            _netView = netView;
        }

        public void SpawnUnit(int team)
        {
            FactoryService.GetObjectFromPool(_spawnPoint, _netView.GetOwnerClientId(), _profile.GetUnitProfile().GetUnitType(),team);
        }

        public BuildingProfileSo GetBuildingProfile() => _profile;
        
        
        
    }
}