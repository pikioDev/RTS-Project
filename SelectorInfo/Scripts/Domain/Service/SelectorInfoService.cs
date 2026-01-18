using System;
using UnityEngine;
using Xenocode.Features.BuildCreator.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.SelectorInfo.Scripts.Domain.Service
{
    public class SelectorInfoService : ISelectorInfoService
    {
        private readonly BuildingsDatabaseSO _buildingsDatabase;
        private readonly UnitProfileList _unitsDatabase; 
        
        private BuildingType _focusedBuildingType = BuildingType.None;
        private UnitType _focusedUnitType = UnitType.None;

        public SelectorInfoService()
        {
            _buildingsDatabase = Resources.Load<BuildingsDatabaseSO>("BuildingDatabase");
            _unitsDatabase = Resources.Load<UnitProfileList>("UnitsProfileList"); 
        }
        
        public BuildingProfileSo GetFocusedBuildingProfile(BuildingType type) => _buildingsDatabase.GetProfileByType(type);


        public void FocusOnUnit(UnitType type)
        {
            _focusedUnitType = type;
            _focusedBuildingType = BuildingType.None; 
        }
        
        public void FocusOnBuilding(BuildingType type)
        {
            _focusedBuildingType = type;
            _focusedUnitType = UnitType.None; 
        }

        public void ClearFocus()
        {
            _focusedBuildingType = BuildingType.None;
            _focusedUnitType = UnitType.None;
        }
    }
}