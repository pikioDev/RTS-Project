using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Model;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Presentation;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Service;
using Xenocode.Features.SuppliesManager.Settings;

namespace Xenocode.Features.SuppliesManager.Scripts.Domain.Provider
{
    public static class SuppliesManagerProvider
    {
        private static ISuppliesService _suppliesService;
   
        public static void Present(ISuppliesManager manager, MatchSettingsSO config) => 
            new SuppliesManagerPresenter(manager, GetSuppliesService(config), MatchProvider.GetMatchService());

        private static ISuppliesService GetSuppliesService(MatchSettingsSO config) => 
            _suppliesService ??= new SuppliesManagerService(config);
        
        public static ISuppliesService GetService() => _suppliesService;
    }
}