using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Building.Scripts.Domain.Presentation;
using Xenocode.Features.Building.Scripts.Domain.Services;
using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.WorldSelector.Scripts.Domain.Provider;

namespace Xenocode.Features.Building.Scripts.Domain.Providers
{
    public static class BuildingProvider
    {
        public static void Present(IBuildingView view, IBuildingNetView netView, BuildingProfileSo profile) => 
            new BuildingPresenter(view, netView,GetBuildingService(profile), WorldSelectorProvider.GetWorldSelectorService(), MatchProvider.GetMatchService());
         
        private static BuildingOperationService GetBuildingService(BuildingProfileSo profile) => new(profile);
        
        public static void SelectBuildOnSpawn(IBuildingNetView view) 
        {
            WorldSelectorProvider.GetWorldSelectorService().SelectBuilding(view);
        }
    }
}