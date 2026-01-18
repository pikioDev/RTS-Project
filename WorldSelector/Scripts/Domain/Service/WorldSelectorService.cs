using System;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.WorldSelector.Scripts.Domain.Model;

namespace Xenocode.Features.WorldSelector.Scripts.Domain.Service
{
    public class WorldSelectorService : IWorldSelectorService
    {
        public event Action<INetworkUnit> OnUnitSelected;
        public event Action<IBuildingNetView> OnBuildingSelected;
        public event Action<Vector3> OnTerrainClicked;
        public event Action<BuildingProfileSo, string> OnBuildingInfoUpdate;
        public event Action OnCancelClick;
        
        public void SelectUnit(INetworkUnit networkUnit) => OnUnitSelected?.Invoke(networkUnit);
        public void SelectBuilding(IBuildingNetView building) => OnBuildingSelected?.Invoke(building);
        public void ClickTerrain(Vector3 pos) => OnTerrainClicked?.Invoke(pos);
        public void NotifyBuildingUpdate(BuildingProfileSo profile, string timer) => OnBuildingInfoUpdate?.Invoke(profile, timer);
        public void CancelClick() => OnCancelClick?.Invoke();
    }
}