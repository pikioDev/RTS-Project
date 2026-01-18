using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Model;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Provider;

namespace Xenocode.Features.ScreenSettings.Scripts.Delivery
{
    public class ScreenSettingsView : MonoBehaviour, IScreenSettingsView
    {
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private TMP_Dropdown _screenModeDropdown;
        [SerializeField] private TMP_Dropdown _vsyncDropdown;
        [SerializeField] private Slider _framerateSlider;
        [SerializeField] private TMP_Text _framerateText;
        
        public UnityEvent<int> OnResolutionDropdownChanged() => _resolutionDropdown.onValueChanged;
        public UnityEvent<int> OnQualityDropdownChanged() => _qualityDropdown.onValueChanged;
        public UnityEvent<int> OnScreenModeDropdownChanged() => _screenModeDropdown.onValueChanged;
        public UnityEvent<int> OnVsyncDropdownChanged() => _vsyncDropdown.onValueChanged;
        public UnityEvent<float> OnFramerateSliderChanged() => _framerateSlider.onValueChanged;
        
        void Start()
        {
            ScreenSettingsProvider.Present(this);
        }
        public void ClearDropdown()
        {
            _resolutionDropdown.ClearOptions();
        }
        public void FillDropdown(List<string> options)
        {
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.RefreshShownValue();
            _qualityDropdown.RefreshShownValue();
            _screenModeDropdown.RefreshShownValue();
        }

        public void SetQuality(int quality) => _qualityDropdown.SetValueWithoutNotify(quality);
        public void SetResolution(int resolution) => _resolutionDropdown.SetValueWithoutNotify(resolution);
        public void SetScreenMode(int mode) => _screenModeDropdown.SetValueWithoutNotify(mode);
        public void SetVsyncState(int vsync) => _vsyncDropdown.SetValueWithoutNotify(vsync);
        public void SetFramerate(float framerate)
        {
            _framerateSlider.SetValueWithoutNotify(framerate);
            _framerateText.text = framerate.ToString();
        }
    }
}