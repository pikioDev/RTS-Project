using Xenocode.Features.WorldSelector.Scripts.Domain.Model;
using Xenocode.Features.WorldSelector.Scripts.Domain.Presentation;
using Xenocode.Features.WorldSelector.Scripts.Domain.Service;

namespace Xenocode.Features.WorldSelector.Scripts.Domain.Provider
{
    public static class WorldSelectorProvider
    {
        private static IWorldSelectorService _worldSelectorService;
        public static void Present(IWorldSelectorView view) => new WorldSelectorPresenter(view, GetWorldSelectorService());

        public static IWorldSelectorService GetWorldSelectorService() =>
            _worldSelectorService ??= new WorldSelectorService();

    }
}