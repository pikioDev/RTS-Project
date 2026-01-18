using Xenocode.Features.PlayerPref.Scripts.Domain.Providers;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Model;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Presenter;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Services;

namespace Xenocode.Features.ScreenSettings.Scripts.Domain.Provider
{
    public static class ScreenSettingsProvider
    {
        private static IScreenOptionsService _screenOptionsService;
        
        public static void Present(IScreenSettingsView view) =>
            new ScreenSettingsPresenter(view, GetScreenOptionsService());
        
        private static IScreenOptionsService GetScreenOptionsService() => 
            _screenOptionsService ??= new ScreenOptionsService(SettingsRepositoryProvider.GetSettingsRepository());
    }
}
