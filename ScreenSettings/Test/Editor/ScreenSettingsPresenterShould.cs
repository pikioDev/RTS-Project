using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Model;
using Xenocode.Features.ScreenSettings.Scripts.Domain.Presenter;

namespace Xenocode.Features.ScreenSettings.Test.Editor
{
    public class ScreenSettingsPresenterShould
    {
        private readonly UnityEvent<int> _onResolutionDropdownChanged = new();
        private readonly UnityEvent<int> _onQualityDropdownChanged = new();
        private readonly UnityEvent<int> _onScreenModeDropdownChanged = new();
        private readonly UnityEvent<int> _onVsyncDropdownChanged = new();
        private readonly UnityEvent<float> _onFramerateSliderValueChanged = new();
        
        private IScreenSettingsView _view;
        private IScreenOptionsService _screenService;
        private ISettingsRepository _repository;
        private PlayerSettings _settings;

        [SetUp]
        public void Setup()
        {
            _view = Substitute.For<IScreenSettingsView>();
            _screenService = Substitute.For<IScreenOptionsService>();
            
            _view.OnResolutionDropdownChanged().Returns(_onResolutionDropdownChanged);
            _view.OnQualityDropdownChanged().Returns(_onQualityDropdownChanged);
            _view.OnScreenModeDropdownChanged().Returns(_onScreenModeDropdownChanged);
            _view.OnVsyncDropdownChanged().Returns(_onVsyncDropdownChanged);
            _view.OnFramerateSliderChanged().Returns(_onFramerateSliderValueChanged);
        }
        
        [Test]
        public void InitializeViewByCleaningDropdown()
        {
            GivenAResolutionDropdown();
            WhenAPresenterIsCreated();
            ThenViewDropdownIsCleaned();
        }

        [Test]
        public void InitializeViewResolutionDropdownFill()
        {
            var list = new List<string> { "1920" , "720"};
            GivenAListOfResolution(list);
            WhenAPresenterIsCreated();
            ThenViewResolutionDropdownIsFilled(list);
        }

        [Test]
        public void InitializeViewQualityDropdown()
        {
            var index = 1;
            GivenAQualityDropdown(index);
            WhenAPresenterIsCreated();
            ThenViewQualityDropdownIsSet(index);
        }

        [Test]
        public void InitializeViewResolutionDropdown()
        {
            var index = 0;
            GivenAResolutionDropdown();
            WhenAPresenterIsCreated();
            ThenViewResolutionDropdownIsSet(index);
        }
        
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void InitializeViewScreenModeDropdown(int index)
        {
            GivenAScreenModeDropdown(index);
            WhenAPresenterIsCreated();
            ThenViewScreenModeDropdownIsSet(index);
        }

        [TestCase(0)]
        public void InitializeViewVsyncDropdown(int index)
        {
            GivenAVsyncDropdown(index);
            WhenAPresenterIsCreated();
            ThenViewVsyncDropdownIsSet(index);
        }

        [Test]
        public void InitializeViewFramerateSliderAndText()
        {
            var index = 60;
            GivenAResolutionSlider(index);
            WhenAPresenterIsCreated(); 
            ThenViewResolutionSliderAndTextIsSet(index);
        }

        [Test]
        public void InitializeGraphicsQualitySettings()
        {
            WhenAPresenterIsCreated();
            ThenGraphicsQualityIsInitialized();
        }

        [Test]
        public void InitializeResolutionSettings()
        {
            WhenAPresenterIsCreated();
            ThenScreenResolutionIsInitialized();
        }

        [Test]
        public void InitializeFramerateSettings()
        {
            WhenAPresenterIsCreated();
            ThenFramerateIsInitialized();
        }

        [Test]
        public void InitializeVsyncSettings()
        {
            WhenAPresenterIsCreated();
            ThenVsyncSettingsIsInitialized();
        }

        [Test]
        public void InitializeScreenModeSettings()
        {
            WhenAPresenterIsCreated();
            ThenScreenModeIsInitialized();
        }

        [Test]
        public void ChangeScreenResolution()
        {
            var value = 1;
            GivenAPresenter();
            WhenResolutionDropdownIsChanged(value);
            ThenScreenResolutionInSet(value);
        }

        [Test]
        public void ChangeGraphicsQuality()
        {
            var value = 1;
            GivenAPresenter();
            WhenQualityDropdownIsChanged(value);
            ThenGraphicsQualityIsSet(value);
        }

        [Test]
        public void ChangeScreenMode()
        {
            var value = 1;
            GivenAPresenter();
            WhenScreenModeDropdownIsChanged(value);
            ThenScreenModeIsSet(value);
        }

        [Test]
        public void ChangeVsyncState()
        {
            var value = 1;
            GivenAPresenter();
            WhenVsyncDropdownIsChanged(value);
            ThenVsyncStateIsSet(value);
        }

        [Test]
        public void ChangeFramerate()
        {
            var value = 60;
            GivenAPresenter();
            WhenFramerateSliderIsChanged(value);
            ThenFramerateIsSet(value);
        }
        
        private void GivenAResolutionDropdown()
        {
            _screenService.GetResolutionIndex().Returns(0);
        }
        
        private void GivenAPresenter()
        {
            var presenter = new ScreenSettingsPresenter(_view, _screenService);
        }
        
        private void GivenAResolutionSlider(int index)
        {
            _screenService.GetFrameRateIndex().Returns(index);
        }

        private void GivenAVsyncDropdown(int index)
        {
            _screenService.GetVsyncState().Returns(index);
        }

        private void GivenAScreenModeDropdown(int index)
        {
            _screenService.GetScreenMode().Returns(index);
        }
        
        private void GivenAListOfResolution(List<string> index)
        {
            _screenService.GetResolutionsList().Returns(index);
        }
        
        private void GivenAQualityDropdown(int index)
        {
            _screenService.GetQualityIndex().Returns(index);
        }
        
        private void WhenAPresenterIsCreated() => GivenAPresenter();
        
        private void WhenVsyncDropdownIsChanged(int value) => _onVsyncDropdownChanged.Invoke(value);

        private void WhenFramerateSliderIsChanged(int value) => _onFramerateSliderValueChanged.Invoke(value);

        private void WhenScreenModeDropdownIsChanged(int value) => _onScreenModeDropdownChanged.Invoke(value);

        private void WhenQualityDropdownIsChanged(int value) => _onQualityDropdownChanged.Invoke(value);

        private void WhenResolutionDropdownIsChanged(int value) => _onResolutionDropdownChanged.Invoke(value);

        private void ThenFramerateIsSet(int value)
        {
            _screenService.Received(1).SetFrameRate(value);
        }
        
        private void ThenVsyncStateIsSet(int value)
        {
            _screenService.Received(1).SetVSync(value);
        }

        private void ThenScreenModeIsSet(int value)
        {
            _screenService.Received(1).SetScreenMode(value);
        }
        
        private void ThenGraphicsQualityIsSet(int value)
        {
            _screenService.Received(1).SetQuality(value);
        }

        private void ThenScreenResolutionInSet(int value)
        {
            _screenService.Received(1).SetResolution(value);
        }
        
        private void ThenScreenModeIsInitialized()
        {
            _screenService.Received(1).InitializeScreenMode();
        }
        
        private void ThenVsyncSettingsIsInitialized()
        {
            _screenService.Received(1).InitializeVsyncState();
        }

        private void ThenFramerateIsInitialized()
        {
            _screenService.Received(1).InitializeFramerate();
        }

        private void ThenScreenResolutionIsInitialized()
        {
            _screenService.Received(1).InitializeResolution(); 
        }

        private void ThenGraphicsQualityIsInitialized()
        {
            _screenService.Received(1).InitializeGraphicsQuality();
        }
        
        private void ThenViewScreenModeDropdownIsSet(int index)
        {
            _view.Received(1).SetScreenMode(index);
        }
        
        private void ThenViewResolutionDropdownIsSet(int index)
        {
            _view.Received(1).SetResolution(index);
        }
        
        private void ThenViewQualityDropdownIsSet(int value)
        {
            _view.Received(1).SetQuality(value);
        }
        
        private void ThenViewDropdownIsCleaned()
        {
            _view.Received(1).ClearDropdown();
        }
        
        private void ThenViewResolutionDropdownIsFilled(List<string> index)
        {
            _view.Received(1).FillDropdown(index);
        }
        
        private void ThenViewVsyncDropdownIsSet(int index)
        {
            _view.Received(1).SetVsyncState(index);
        }
        
        private void ThenViewResolutionSliderAndTextIsSet(int index)
        {
            _view.Received(1).SetFramerate(index);
        }
    }
}
