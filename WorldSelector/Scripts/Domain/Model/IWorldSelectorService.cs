using System;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.WorldSelector.Scripts.Domain.Model
{
    public interface IWorldSelectorService
    {
        event Action<INetworkUnit> OnUnitSelected;

        event Action<IBuildingNetView> OnBuildingSelected;
        
        event Action<Vector3> OnTerrainClicked;

        event Action OnCancelClick;
        
        event Action<BuildingProfileSo, string> OnBuildingInfoUpdate;
        
        void SelectUnit(INetworkUnit networkUnit);
   
        void SelectBuilding(IBuildingNetView building);
        
        void ClickTerrain(Vector3 position);

        public void NotifyBuildingUpdate(BuildingProfileSo profile, string timer);

        void CancelClick();

    }
}