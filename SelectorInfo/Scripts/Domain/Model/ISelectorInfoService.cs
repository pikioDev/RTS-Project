using System;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.SelectorInfo.Scripts.Domain.Model
{
    public interface ISelectorInfoService
    {
        BuildingProfileSo GetFocusedBuildingProfile(BuildingType type);
       
        void FocusOnUnit(UnitType type);
    
        void FocusOnBuilding(BuildingType type);
       
        void ClearFocus();
    }
}