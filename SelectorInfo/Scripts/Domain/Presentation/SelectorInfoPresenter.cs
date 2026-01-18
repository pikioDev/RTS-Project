using System;
using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Services;
using Xenocode.Features.WorldSelector.Scripts.Domain.Model;

namespace Xenocode.Features.SelectorInfo.Scripts.Domain.Presentation
{
    public class SelectorInfoPresenter
    {
        private readonly ISelectorInfoView _view;
        private readonly ISelectorInfoService _selectorInfoService;
        private readonly IBuildSelectorView _buildingsSelectorView;
        private readonly IWorldSelectorService _worldSelectorService;
        private readonly IUnitService _unitService;
        private readonly IAudioEvents _audioService;
        
        private Guid _guid;
        private INetworkUnit _netUnit;
        private UnitProfile _unitProfile;
        
        private IBuildingNetView _netBuild;
        private BuildingProfileSo _buildProfile;


        public SelectorInfoPresenter(ISelectorInfoView view, ISelectorInfoService selectorInfoService,
            IWorldSelectorService worldSelectorService, IUnitService unitService, IAudioEvents audioService)
        {
            _view = view;
            _selectorInfoService = selectorInfoService;
            _worldSelectorService = worldSelectorService;
            _unitService = unitService;
            _audioService = audioService;
            SubscribeToWorldSelectorService();
        }
        
        private void SubscribeToWorldSelectorService()
        {
            _worldSelectorService.OnTerrainClicked += HandleTerrainClicked;
            _worldSelectorService.OnBuildingSelected += HandleBuildingSelected;
            _worldSelectorService.OnUnitSelected += HandleUnitSelected;
            _worldSelectorService.OnCancelClick += HandleCancelClick;
        }

        private void HandleCancelClick()
        {
            _selectorInfoService.ClearFocus();
            _view.HideInfo();
        }

        private void HandleTerrainClicked(Vector3 obj)
        {
            ClearUnitSubscriptions();
            ClearBuildingSubscriptions();
            _view.HideInfo();
            _selectorInfoService.ClearFocus();
            _buildProfile = null;
        }

        private void HandleBuildingSelected(IBuildingNetView build)
        {
            ClearUnitSubscriptions();   
            ClearBuildingSubscriptions(); 
    
            _selectorInfoService.FocusOnBuilding(build.GetBuildingProfile().BuildingType());
            _buildProfile = build.GetBuildingProfile();
            
            _netBuild = build;
            _netBuild.OnTimerRemainingChanged += HandleBuildingTimerChanged;
            _netBuild.OnHitsPointsChanged += HandleBuildingHitPointsChanged;
            
            _view.ShowDescription(_buildProfile.GetDescription());
            HandleBuildingTimerChanged(_netBuild.GetTimerRemaining());
            HandleBuildingHitPointsChanged((int)build.GetHitPoints());
            
            if (_netBuild.IsAlly())
                _audioService.PlaySound(_buildProfile.GetAudioProfile().name, (int)AudioClipType.Select, _netBuild.GetPosition());
        }

      

        private void HandleBuildingTimerChanged(float newTimer)
        {
            var timerFormatted = $"{ (int)newTimer} / {_buildProfile.GetSpawnInterval()}";
            _view.ShowSpawnTimer(timerFormatted);
        }
        
        private void HandleBuildingHitPointsChanged(int newHp)
        {
            var hpFormatted = $"{newHp} / {_buildProfile.GetMaxHitPoints()}";
            _view.ShowBuildingHitPoints(hpFormatted);
        }
        
        private void HandleUnitSelected(INetworkUnit networkUnitView)
        {
            ClearUnitSubscriptions();
            ClearBuildingSubscriptions();
            
            _selectorInfoService.FocusOnUnit(networkUnitView.GetUnitType());
            _unitProfile = _unitService.GetUnitProfileByUnitType(networkUnitView.GetUnitType()); 
            _netUnit = networkUnitView;
            _netUnit.ShowHighLight();
            _guid = networkUnitView.GetGuid();
            
            _netUnit.OnUnitHitPointsChanged += HandleHitPointsUpdate;
            _view.ShowUnitStats(_unitProfile);
            HandleHitPointsUpdate(_guid, (int)_netUnit.GetCurrentNetworkHealth());
            
            if (_netUnit.IsAlly())
                _audioService.PlaySound(_unitProfile.AudioProfile.name, (int)AudioClipType.Select, _netUnit.GetPosition());
        }
        
        private void HandleHitPointsUpdate(Guid guid, int hp)
        {
            if (guid == _guid)
            {
                var hpFormatted = $"{hp} / {_unitProfile.MaxHealth}";
                _view.ShowUnitHitPoints(hpFormatted);
            }
        }
        
        private void ClearUnitSubscriptions()
        {
            if (_netUnit != null)
            {
                _netUnit.OnUnitHitPointsChanged -= HandleHitPointsUpdate;
                _netUnit.HideHighLight();
                _netUnit = null; 
                _view.HideInfo();
            }
            _guid = Guid.Empty;
          
        }
        
        private void ClearBuildingSubscriptions()
        {
            if (_netBuild != null)
            {
                _netBuild.OnTimerRemainingChanged -= HandleBuildingTimerChanged;
                _netBuild.OnHitsPointsChanged -= HandleBuildingHitPointsChanged;
                _view.HideInfo();
                _netBuild = null;
            }
        }
        
        private void PlaySoundEffect()
        {
            
        }
    }
}