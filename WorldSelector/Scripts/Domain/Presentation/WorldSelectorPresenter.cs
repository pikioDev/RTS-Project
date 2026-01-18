using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.WorldSelector.Scripts.Domain.Model;

namespace Xenocode.Features.WorldSelector.Scripts.Domain.Presentation
{
    public class WorldSelectorPresenter
    {
        private readonly IWorldSelectorView _view;
        private readonly IWorldSelectorService _worldSelectorService;
        private INetworkUnit _networkUnit;
        private IBuildingNetView _building;

        public WorldSelectorPresenter(IWorldSelectorView view, IWorldSelectorService worldSelectorServiceService)
        {
            _view = view;
            _worldSelectorService = worldSelectorServiceService;
            SubscribeToViewEvents();
        }

        private void SubscribeToViewEvents()
        {
            _view.OnBuildingClicked += HandleBuildingClicked;
            _view.OnUnitClicked += HandleUnitClicked;
            _view.OnTerrainClicked += HandleTerrainClicked;
            _view.OnCancelClick += HandleCancelClick;
        }

        private void HandleCancelClick()
        {
            _worldSelectorService.CancelClick();
        }

        private void HandleTerrainClicked(Vector3 pos)
        {
            _worldSelectorService.ClickTerrain(pos);
        }

        private void HandleUnitClicked(INetworkUnit networkUnit)
        {
            _worldSelectorService.SelectUnit(networkUnit);
        }

        private void HandleBuildingClicked(IBuildingNetView building)
        {
         
            _building = building;
            _worldSelectorService.SelectBuilding(building);
        }
        
    }
}