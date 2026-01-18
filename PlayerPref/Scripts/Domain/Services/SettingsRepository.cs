using UnityEngine;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;

namespace Xenocode.Features.PlayerPref.Scripts.Domain.Services

{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IPlayerPrefsService _prefsService;
        private PlayerSettings _currentSettings;

        public SettingsRepository(IPlayerPrefsService prefsService)
        {
            _prefsService = prefsService;
        }
        
        public PlayerSettings GetSettings()
        {
            _currentSettings ??= new PlayerSettings
            {
                MasterVolume = _prefsService.LoadFloat(PrefsKeys.MasterVolume),
                SfxVolume = _prefsService.LoadFloat(PrefsKeys.SfxVolume),
                MusicVolume = _prefsService.LoadFloat(PrefsKeys.MusicVolume),
                IsMuted = _prefsService.LoadBool(PrefsKeys.IsMuted),
                QualityIndex = _prefsService.LoadInt(PrefsKeys.QualityIndex, QualitySettings.GetQualityLevel()),
                ResolutionIndex = _prefsService.LoadInt(PrefsKeys.ResolutionIndex),
                ScreenMode = _prefsService.LoadInt(PrefsKeys.ScreenMode, GetDefaultScreenModeIndex()),
                VSync =  _prefsService.LoadInt(PrefsKeys.VSync),
                Framerate = _prefsService.LoadInt(PrefsKeys.Framerate, 60),
            };
            
            return _currentSettings;
        }

        public void SaveAllSettings()
        {
            _prefsService.SaveFloat(PrefsKeys.MasterVolume, _currentSettings.MasterVolume);
            _prefsService.SaveFloat(PrefsKeys.SfxVolume, _currentSettings.SfxVolume);
            _prefsService.SaveFloat(PrefsKeys.MusicVolume, _currentSettings.MusicVolume);
            _prefsService.SaveBool(PrefsKeys.IsMuted, _currentSettings.IsMuted);
            _prefsService.SaveInt(PrefsKeys.QualityIndex, _currentSettings.QualityIndex);
            _prefsService.SaveInt(PrefsKeys.ResolutionIndex, _currentSettings.ResolutionIndex);
            _prefsService.SaveInt(PrefsKeys.ScreenMode, _currentSettings.ScreenMode);
            _prefsService.SaveInt(PrefsKeys.VSync, _currentSettings.VSync);
            _prefsService.SaveInt(PrefsKeys.Framerate, _currentSettings.Framerate);
            _prefsService.Save();
        }

        private int GetDefaultScreenModeIndex()
        {
            return Screen.fullScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 0,
                FullScreenMode.FullScreenWindow => 1,
                FullScreenMode.Windowed => 2,
                _ => 0
            };
        }
    }
}