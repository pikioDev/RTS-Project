using Xenocode.Features.Building.Scripts.Delivery;

namespace Xenocode.Features.BuildSelector.Scripts.Domain.Model
{
    public interface IBuildSelectorView
    {
        BuildButton[] GetBuildButtons();
    }
}