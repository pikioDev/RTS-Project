using System;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;

namespace Xenocode.Features.BuildSelector.Scripts.Domain.Service
{
    public class BuildSelectorService : IBuildSelectorService
    {
        private BuildingType _currentBuildingType =  BuildingType.None;
        public event Action<BuildingType> OnButtonHighlight;
        public event Action OnButtonUnhighlight;
        
        private bool _isSelected;
        public void SelectBuilding(BuildingType type)
        {
            _currentBuildingType = type;
            _isSelected = true;
        }

        public void HighlightButton(BuildingType type)
        {
            OnButtonHighlight?.Invoke(type);
        }

        public void UnhighlightButton()
        {
            OnButtonUnhighlight?.Invoke();
        }

        public void DeselectBuilding()
        {
            _isSelected = false;
        }

        public BuildingType GetSelectedType() => _currentBuildingType;
        public bool IsButtonSelected() => _isSelected;
    }
}