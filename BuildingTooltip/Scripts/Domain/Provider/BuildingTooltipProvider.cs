using Xenocode.Features.BuildingTooltip.Scripts.Domain.Model;
using Xenocode.Features.BuildingTooltip.Scripts.Domain.Presentation;
using Xenocode.Features.BuildSelector.Scripts.Domain.Provider;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Provider;

namespace Xenocode.Features.BuildingTooltip.Scripts.Domain.Provider
{
    public static class BuildingTooltipProvider
    {
        public static void Present(IBuildTooltipView view) =>
            new BuildTooltipPresenter(view, BuildSelectorProvider.GetBuildSelectorService(),
                SelectorInfoProvider.GetSelectorInfoService()); 
    }
}