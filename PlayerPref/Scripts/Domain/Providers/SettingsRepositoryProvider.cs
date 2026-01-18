using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.PlayerPref.Scripts.Domain.Services;

namespace Xenocode.Features.PlayerPref.Scripts.Domain.Providers
{
    public static class SettingsRepositoryProvider
    {
        private static ISettingsRepository _repository;
        private static IPlayerPrefsService _prefsService;
        public static  ISettingsRepository GetSettingsRepository() => 
            _repository ??= new SettingsRepository(GetPlayerPrefsService());
        private static IPlayerPrefsService GetPlayerPrefsService() => 
            _prefsService ??= new PlayerPrefsService();
    }
}