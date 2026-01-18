using NSubstitute;
using NUnit.Framework;
using UnityEngine.Events;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Presenter;

namespace Xenocode.Features.SoundSettings.Test.Editor
{
    public class SoundSettingsPresenterShould
    {
        private readonly UnityEvent<float> _onMasterSliderChanged = new UnityEvent<float>();
        private readonly UnityEvent<float> _onSfxSliderChanged = new UnityEvent<float>();
        private readonly UnityEvent<float> _onMusicSliderChanged = new UnityEvent<float>();
        private readonly UnityEvent<bool> _onMuteToggleChanged = new UnityEvent<bool>();
        private ISoundSettingsView _view;
        private IAudioMixerService _mixerService;

        [SetUp]
        public void Setup()
        {
            _view = Substitute.For<ISoundSettingsView>();
            _mixerService = Substitute.For<IAudioMixerService>();

            _view.OnMasterSliderChanged().Returns(_onMasterSliderChanged);
            _view.OnSfxSliderChanged().Returns(_onSfxSliderChanged);
            _view.OnMusicSliderChanged().Returns(_onMusicSliderChanged);
            _view.OnMuteToggleChanged().Returns(_onMuteToggleChanged);
        }

        private void GivenAPresenter()
        {
            var presenter = new SoundSettingsPresenter(_view, _mixerService);
        }

        [Test]
        public void InitializeMixerMasterVolume()
        {
            WhenAPresenterIsCreated();
            ThenMasterVolumeIsInitialized();
        }

        [Test]
        public void InitializeMixerSfxVolume()
        {
            WhenAPresenterIsCreated();
            ThenSfxVolumeIsInitialized();
        }

        [Test]
        public void InitializeMixerMusicVolume()
        {
            WhenAPresenterIsCreated();
            ThenMusicVolumeIsInitialized();
        }

        [Test]
        public void InitializeMixerMuteToggle()
        {
            WhenAPresenterIsCreated();
            ThenMuteStateIsInitialized();
        }

        [TestCase(0)] 
        [TestCase(0.2f)] 
        [TestCase(0.5f)] 
        [TestCase(0.8f)] 
        [TestCase(1f)] 
        public void InitializeViewMasterVolume(float returnValue)
        {
            GivenAMasterVolume(returnValue);
            WhenAPresenterIsCreated();
            ThenViewMasterVolumeIsInitialized(returnValue);
        }

        [Test]
        public void InitializeViewSfxVolume()
        {
            var value = 0.6f;
            GivenASfxVolume(value);
            WhenAPresenterIsCreated();
            ThenViewSfxVolumeIsInitialized(value);
        }

        [Test]
        public void InitializeViewMusicVolume()
        {
            var value = 0.6f;
            GivenAMusicVolume(value);
            WhenAPresenterIsCreated();
            ThenViewMusicVolumeIsInitialized(value);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void InitializeViewMuteToggle(bool returnValue)
        {
            GivenAMuteState(returnValue);
            WhenAPresenterIsCreated();
            ThenViewMuteStateIsInitialized(returnValue);
        }

        [Test]
        public void ChangeMixerMasterVolume()
        {   
            var returnValue = 1.0f;
            GivenAPresenter();
            WhenMasterSliderValueIsChanged(returnValue);
            ThenMasterVolumeIsUpdated(returnValue);
        }

        [Test]
        public void ChangeMixerSfxVolume()
        {
            var returnValue = 1.0f;
            GivenAPresenter();
            WhenSfxSliderValueIsChanged(returnValue);
            ThenSfxVolumeIsUpdated(returnValue);
        }

        [Test]
        public void ChangeMixerMusicVolume()
        {
            var returnValue = 1.0f;
            GivenAPresenter();
            WhenMusicSliderValueIsChanged(returnValue);
            ThenMusicVolumeIsUpdated(returnValue);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ChangeMixerMuteToggle(bool returnValue)
        {
            GivenAPresenter();
            WhenMuteToggleIsTriggered(returnValue);
            ThenMuteStateIsUpdate(returnValue);
        }
        
        private void GivenAMusicVolume(float returnValue) => _mixerService.GetMusicVolume().Returns(returnValue);
        
        private void GivenASfxVolume(float returnValue) => _mixerService.GetSfxVolume().Returns(returnValue);
        
        private void GivenAMuteState(bool returnValue) => _mixerService.GetMutedState().Returns(returnValue);
        
        private void GivenAMasterVolume(float returnsValue) => _mixerService.GetMasterVolume().Returns(returnsValue);
        
        private void WhenAPresenterIsCreated() => GivenAPresenter();

        private void WhenMusicSliderValueIsChanged(float value) => _onMusicSliderChanged.Invoke(value);

        private void WhenMasterSliderValueIsChanged(float value) => _onMasterSliderChanged.Invoke(value);
        
        private void WhenSfxSliderValueIsChanged(float value) => _onSfxSliderChanged.Invoke(value);
        
        private void WhenMuteToggleIsTriggered(bool isMuted) => _onMuteToggleChanged.Invoke(isMuted);
        
        private void ThenViewMuteStateIsInitialized(bool value) => _view.Received(1).SetMuteToggle(value);

        private void ThenViewMusicVolumeIsInitialized(float value) => _view.Received(1).SetMusicSlider(value);
        
        private void ThenViewSfxVolumeIsInitialized(float value) => _view.Received(1).SetSfxSlider(value);
        
        private void ThenViewMasterVolumeIsInitialized(float value) => _view.Received(1).SetMasterSlider(value);
        
        private void ThenMuteStateIsInitialized() => _mixerService.Received(1).InitializeToggleMute();

        private void ThenMusicVolumeIsInitialized() => _mixerService.Received(1).InitializeMusicVolume();

        private void ThenSfxVolumeIsInitialized() => _mixerService.Received(1).InitializeSfxVolume();

        private void ThenMasterVolumeIsInitialized() => _mixerService.Received(1).InitializeMasterVolume();
        
        private void ThenMusicVolumeIsUpdated(float returnValue) => _mixerService.Received(1).SetMusicVolume(returnValue);

        private void ThenSfxVolumeIsUpdated(float returnValue) => _mixerService.Received(1).SetSfxVolume(returnValue);

        private void ThenMasterVolumeIsUpdated(float returnValue) => _mixerService.Received(1).SetMasterVolume(returnValue);
        
        private void ThenMuteStateIsUpdate(bool returnValue) => _mixerService.Received(1).SetToggleMute(returnValue);
    } 
}
