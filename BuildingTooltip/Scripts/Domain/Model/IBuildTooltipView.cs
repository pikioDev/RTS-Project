using Xenocode.Features.Building.Scripts.Domain.Model;

namespace Xenocode.Features.BuildingTooltip.Scripts.Domain.Model
{
    public interface IBuildTooltipView
    {
        void ShowTooltip(BuildingType type, BuildingProfileSo profiles);
        void HideTooltip();
    }
}