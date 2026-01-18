using System.Collections.Generic;
using UnityEngine.Events;

namespace Xenocode.Features.ScreenSettings.Scripts.Domain.Model
{
    public interface IScreenSettingsView
    {
        public UnityEvent<int> OnResolutionDropdownChanged();
        public UnityEvent<int> OnQualityDropdownChanged();
        public UnityEvent<int> OnScreenModeDropdownChanged();
        public UnityEvent<int> OnVsyncDropdownChanged();
        public UnityEvent<float> OnFramerateSliderChanged();
        public void FillDropdown(List<string> options);
        public void ClearDropdown();
        public void SetQuality(int quality);
        public void SetResolution(int resolution);
        public void SetScreenMode(int mode);
        public void SetVsyncState(int vsync);
        public void SetFramerate(float framerate);
    }
}