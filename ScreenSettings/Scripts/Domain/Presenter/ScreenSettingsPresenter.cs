using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Model;

namespace Xenocode.Features.ScreenSettings.Scripts.Domain.Presenter
{
    public class ScreenSettingsPresenter
    {
        private readonly IScreenSettingsView _view;
        private readonly IScreenOptionsService _screenOptionsService;

        public ScreenSettingsPresenter(IScreenSettingsView view, IScreenOptionsService screenOptionsService)
        {
            _view = view;
            _screenOptionsService = screenOptionsService;
            InitializeView();
            InitializeScreenSettings();
            SubscribeToEvents();
        }
        
        private void InitializeView()
        {
            _view.ClearDropdown();
            _view.FillDropdown(_screenOptionsService.GetResolutionsList());
            _view.SetQuality(_screenOptionsService.GetQualityIndex());
            _view.SetResolution(_screenOptionsService.GetResolutionIndex());
            _view.SetScreenMode(_screenOptionsService.GetScreenMode());
            _view.SetVsyncState(_screenOptionsService.GetVsyncState());
            _view.SetFramerate(_screenOptionsService.GetFrameRateIndex());
        }
        
        private void InitializeScreenSettings()
        {
            _screenOptionsService.InitializeGraphicsQuality();
            _screenOptionsService.InitializeResolution();
            _screenOptionsService.InitializeFramerate();
            _screenOptionsService.InitializeVsyncState();
            _screenOptionsService.InitializeScreenMode();
        }
        
        private void SubscribeToEvents()
        {
            _view.OnResolutionDropdownChanged().AddListener(HandleScreenResolutionChange); 
            _view.OnQualityDropdownChanged().AddListener(HandleQualityChange);  
            _view.OnScreenModeDropdownChanged().AddListener(HandleScreenModeChange);
            _view.OnVsyncDropdownChanged().AddListener(HandleVsyncChange); 
            _view.OnFramerateSliderChanged().AddListener(HandleFramerateChange);
        }
        
        private void HandleQualityChange(int qualityIndex)
        {
            _screenOptionsService.SetQuality(qualityIndex);
        }
        
        private void HandleScreenResolutionChange(int resolutionIndex)
        {
            _screenOptionsService.SetResolution(resolutionIndex);
        }
        
        private void HandleScreenModeChange(int screenModeIndex)
        {
            _screenOptionsService.SetScreenMode(screenModeIndex);
        }
        
        private void HandleVsyncChange(int index)
        {
            _screenOptionsService.SetVSync(index);
        }
        
        private void HandleFramerateChange(float rate)
        {
            _screenOptionsService.SetFrameRate(rate);
            _view.SetFramerate(rate);
        }
    }
}