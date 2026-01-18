using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Model;

namespace Xenocode.Features.ScreenSettings.Scripts.Domain.Services
{
    public class ScreenOptionsService : IScreenOptionsService
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly Resolution[] _resolutionsList;

        public ScreenOptionsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _resolutionsList = Screen.resolutions;

        }
        
        public void InitializeResolution()
        {
            var res = _settingsRepository.GetSettings().ResolutionIndex;
            Screen.SetResolution(_resolutionsList[res].width, _resolutionsList[res].height, Screen.fullScreen); 
        }
        
        public void InitializeGraphicsQuality()
        {
            QualitySettings.SetQualityLevel(_settingsRepository.GetSettings().QualityIndex);
        }

        public void InitializeFramerate()
        {
            Application.targetFrameRate = _settingsRepository.GetSettings().Framerate;
        }

        public void InitializeVsyncState()
        {
            QualitySettings.vSyncCount = _settingsRepository.GetSettings().VSync;
        }

        public void InitializeScreenMode()
        {
            switch (_settingsRepository.GetSettings().ScreenMode)
            {
                case 0: 
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1: 
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2: 
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
        
        public int GetFrameRateIndex() => _settingsRepository.GetSettings().Framerate;

        public int GetVsyncState() => _settingsRepository.GetSettings().VSync;

        public int GetQualityIndex() => _settingsRepository.GetSettings().QualityIndex;

        public int GetScreenMode() => _settingsRepository.GetSettings().ScreenMode;

        public int GetResolutionIndex() => _settingsRepository.GetSettings().ResolutionIndex;
        
        public void SetQuality(int qualityIndex)
        {
            _settingsRepository.GetSettings().QualityIndex = qualityIndex;
            QualitySettings.SetQualityLevel(qualityIndex);
            _settingsRepository.SaveAllSettings();
        }

        public void SetFrameRate(float frameRate)
        {
            _settingsRepository.GetSettings().Framerate = (int)frameRate;
            Application.targetFrameRate = (int)frameRate;
            _settingsRepository.SaveAllSettings();
        }

        public void SetVSync(int index)
        {
            _settingsRepository.GetSettings().VSync = index;
            QualitySettings.vSyncCount = index;
            _settingsRepository.SaveAllSettings();
        }
        
        public void SetResolution(int resolutionIndex)
        {
            _settingsRepository.GetSettings().ResolutionIndex = resolutionIndex;
            Resolution resolution = _resolutionsList[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            _settingsRepository.SaveAllSettings();
        }
        
        public void SetScreenMode(int mode)
        {
            switch (mode)
            {
                case 0: 
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1: 
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2: 
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
        
        public List<string> GetResolutionsList() => 
            _resolutionsList.Select(res => res.width + " x " + res.height).Distinct().ToList();
    }
}
