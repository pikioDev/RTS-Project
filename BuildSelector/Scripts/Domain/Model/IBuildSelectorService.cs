using System;
using Xenocode.Features.Building.Scripts.Domain.Model;

namespace Xenocode.Features.BuildSelector.Scripts.Domain.Model
{
    public interface IBuildSelectorService
    {
        public event Action<BuildingType> OnButtonHighlight;
        public event Action OnButtonUnhighlight;
        public void SelectBuilding(BuildingType type);
        public void HighlightButton(BuildingType type);
        public void UnhighlightButton();
        public void DeselectBuilding();
        public BuildingType GetSelectedType();
        public bool IsButtonSelected();
    }
}