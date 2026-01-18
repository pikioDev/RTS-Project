using Xenocode.Features.Audio.Scripts.Domain.Provider;
using Xenocode.Features.BuildCreator.Scripts.Domain.Model;
using Xenocode.Features.BuildCreator.Scripts.Domain.Presentation;
using Xenocode.Features.BuildCreator.Scripts.Domain.Service;
using Xenocode.Features.BuildSelector.Scripts.Domain.Provider;
using Xenocode.Features.UserSupplies.Scripts.Domain.Provider;
using Xenocode.Features.WorldSelector.Scripts.Domain.Provider;

namespace Xenocode.Features.BuildCreator.Scripts.Domain.Provider
{
    public static class BuildingCreatorProvider
    {
        private static BuildingPlacementService _buildingPlacementService;
        public static void Present(IBuildingCreatorView view)
        {
            
            new BuildingCreatorPresenter(view, GetPlacementService(), BuildSelectorProvider.GetBuildSelectorService(), 
                WorldSelectorProvider.GetWorldSelectorService(), UserSuppliesProvider.GetUserSuppliesService(), AudioProvider.GetAudioEvents());
        }

        public static BuildingPlacementService GetPlacementService() => _buildingPlacementService ??=
            new BuildingPlacementService();
        
    }
}