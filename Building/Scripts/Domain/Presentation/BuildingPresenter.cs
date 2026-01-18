using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Domain.Services;
using Xenocode.Features.Match.Scripts.Domain.Services;
using Xenocode.Features.WorldSelector.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Presentation
{
    public class BuildingPresenter
    {
        private readonly IBuildingView _view;
        private readonly IBuildingNetView _netView;
        private readonly BuildingOperationService _operationService;
        private readonly IWorldSelectorService _worldSelectorService;
        private readonly MatchService _matchService;
        private bool _isSelected = false;

        public BuildingPresenter(IBuildingView view, IBuildingNetView netView,BuildingOperationService getOperationService,
            IWorldSelectorService worldSelectorService, MatchService matchService)
        {
            _view = view;
            _netView = netView;
            _operationService = getOperationService;
            _worldSelectorService = worldSelectorService;
            _matchService = matchService;

            InitializeViewValues();
            
            SubscribeToNetViewEvents();
            SubscribeToServiceEvents();
            SubscribeToSelectorService();
        }

        private void InitializeViewValues()
        {
            _netView.SetHitsPointsNet(_view.GetBuildingProfile().GetMaxHitPoints());
        }

        private void SubscribeToNetViewEvents()
        {
            _netView.OnServerUpdateTimer += HandleServerTick;
            _netView.OnTimerRemainingChanged += HandleNetTimerChanged;
        }

        private void HandleNetTimerChanged(float timeRemaining)
        {
            if (_isSelected)
            {
                NotifyTimerUpdate(timeRemaining);
            }
        }

        private void HandleServerTick()
        {
            _operationService.Tick();
            float currentTime = _operationService.GetTimer();
            _netView.SetTimerRemaining(currentTime);
        }

        private void SubscribeToSelectorService()
        {
            _worldSelectorService.OnBuildingSelected += HandleBuildingSelected;
            _worldSelectorService.OnTerrainClicked += HandleBuildingDeselected;
            _worldSelectorService.OnCancelClick += HandleCancelClick;
        }

        private void HandleCancelClick()
        {
            _isSelected = false;
        }

        private void HandleBuildingDeselected(Vector3 obj)
        {
            _isSelected = false;
        }

        private void HandleBuildingSelected(IBuildingNetView view)
        {
            _isSelected = (view == _netView);
            
            if (_isSelected)
            {
                float currentTimer = _netView.GetTimerRemaining();
                NotifyTimerUpdate(currentTimer);
            }
        }

        private void NotifyTimerUpdate(float timeRemaining)
        {
            var spawnInterval = _view.GetBuildingProfile().GetSpawnInterval();
            var timerFormatted = $"{(int)timeRemaining} / {spawnInterval}"; 
            _worldSelectorService.NotifyBuildingUpdate(_view.GetBuildingProfile(), timerFormatted);
        }

        private void SubscribeToServiceEvents()
        {
            _operationService.OnUnitReadyToSpawn().AddListener(HandleUnitReadyToSpawn);
        }

        private void HandleUnitReadyToSpawn()
        {
            _view.SpawnUnit((int)_matchService.GetTeam(_netView.GetOwnerClientId()));
        }
    }
}