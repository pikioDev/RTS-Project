using Xenocode.Features.Audio.Scripts.Domain.Provider;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Model;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Presentation;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Service;
using Xenocode.Features.Unit.Scripts.Domain.Providers;
using Xenocode.Features.WorldSelector.Scripts.Domain.Provider;


namespace Xenocode.Features.SelectorInfo.Scripts.Domain.Provider
{
    public static class SelectorInfoProvider
    {
        private static ISelectorInfoService _selectorInfoService;
        
        public static void Present(ISelectorInfoView view) => new SelectorInfoPresenter(view, GetSelectorInfoService(), 
            WorldSelectorProvider.GetWorldSelectorService(), UnitProvider.GetUnitService(), AudioProvider.GetAudioEvents());
        
        public static ISelectorInfoService GetSelectorInfoService() => _selectorInfoService ??= new SelectorInfoService();
        
    }
}
