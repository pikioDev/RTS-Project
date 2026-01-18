using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Presenter;
using Xenocode.Features.SoundSettings.Scripts.Domain.Services;

namespace Xenocode.Features.SoundSettings.Test.Editor
{
    public class AudioMixerServiceShould
    {
        private ISettingsRepository _repository;
        private IAudioMixerWrapper _wrapper;
        private IAudioMixerService _service;
        private PlayerSettings _settings;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<ISettingsRepository>();
            _wrapper = Substitute.For<IAudioMixerWrapper>();
            _settings = Substitute.For<PlayerSettings>();
            _repository.GetSettings().Returns(_settings);
        }

        [Test]
        public void InitializeMasterVolumeValue()
        {
            var testValue = 0.5f;
            var expectedDbValue = Mathf.Log10(testValue) * 20;
            GivenAService();
            GivenMasterVolumeInRepository(testValue);
            WhenInitializeMasterVolumeIsCalled();
            ThenMasterVolumeIsApplied("Master", expectedDbValue);
        }

        [Test]
        public void InitializeSfxVolumeValue()
        {
            var testValue = 0.5f;
            var expectedDbValue = Mathf.Log10(testValue) * 20;
            GivenAService();
            GivenASfxValueInRepository(testValue);
            WhenInitializeSfxVolumeIsCalled();
            ThenSfxVolumeIsApplied("Sfx", expectedDbValue);
        }

        [Test]
        public void InitializeMusicVolumeValue()
        {
            var testValue = 0.5f;
            var expectedDbValue = Mathf.Log10(testValue) * 20;
            GivenAService();
            GivenAMusicValueInRepository(testValue);
            WhenInitializeMusicVolumeIsCalled();
            ThenMusicVolumeIsApplied("Music", expectedDbValue);
        }

        [Test]
        public void InitializeToggleMuteOn()
        {
            GivenAService();
            GivenMutedStateInRepository(true);
            WhenInitializeToggleMuteIsCalled();
            ThenMuteStateIsSetOn("Master", -80f);
        }

        [Test]
        public void InitializeToggleMuteOff()
        {
            var masterVolumeValue = 0.75f;
            var expectedDbValue = Mathf.Log10(masterVolumeValue) * 20;
            GivenAService();
            GivenMutedStateInRepository(false);
            GivenMasterVolumeInRepository(masterVolumeValue);
            WhenInitializeToggleMuteIsCalled();
            ThenMuteStateIsSetOff(expectedDbValue);
        }

        [Test]
        public void GetMasterVolumeFromRepository()
        {
            var expectedVolume = 0.8f;
            GivenAService();
            GivenMasterVolumeInRepository(expectedVolume);
            var actualVolume = _service.GetMasterVolume();
            Assert.AreEqual(actualVolume, expectedVolume);
        }

        [Test]
        public void GetSfxVolumeFromRepository()
        {
            var expectedVolume = 0.5f;
            GivenAService();
            GivenASfxValueInRepository(expectedVolume);
            var actualVolume = _service.GetSfxVolume();
            Assert.AreEqual(actualVolume, expectedVolume);
        }
        
        [Test]
        public void GetMusicVolumeFromRepository()
        {
            var expectedVolume = 0.5f;
            GivenAService();
            GivenAMusicValueInRepository(expectedVolume);
            var actualVolume = _service.GetMusicVolume();
            Assert.AreEqual(actualVolume, expectedVolume);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void GetMutedStateFromRepository(bool muted)
        {
            GivenAService();
            GivenMutedStateInRepository(muted);
            var actualState = _service.GetMutedState();
            Assert.AreEqual(actualState, muted);
        }

        [Test]
        public void SetMasterVolume()
        {
            var testValue = 0.8f;
            var expectedDbValue = Mathf.Log10(testValue) * 20;
            GivenAService();
            WhenSetMasterVolumeIsCalled(testValue);
            ThenMasterVolumeIsApplied("Master", expectedDbValue);
        }
        
        [Test]
        public void SetSfxVolume()
        {
            var testValue = 0.8f;
            var expectedDbValue = Mathf.Log10(testValue) * 20;
            GivenAService();
            WhenSetSfxVolumeIsCalled(testValue);
            ThenSfxVolumeIsApplied("Sfx", expectedDbValue);
        }
        
        [Test]
        public void SetMusicVolume()
        {
            var testValue = 0.8f;
            var expectedDbValue = Mathf.Log10(testValue) * 20;
            GivenAService();
            WhenSetMusicVolumeIsCalled(testValue);
            ThenMusicVolumeIsApplied("Sfx", expectedDbValue);
        }
        
        [TestCase(true)]
        [TestCase(false)]
        public void SetMuteToggle(bool mute)
        {
            GivenAService();
            _service.SetToggleMute(true);
            _settings.Received().IsMuted = true;
            _wrapper.Received(1).SetFloat("Master", -80f);
            _repository.Received(1).SaveAllSettings();
        }
        
        [TestCase(1.0f, 0.0f)]
        [TestCase(0.5f, -6.0206f)]
        [TestCase(0.1f, -20.0f)]
        [TestCase(0.0f, -80.0f)] 
        public void GetLogarithmicScale_ConvertsLinearValue_ToCorrectDecibels(float linearValue, float expectedDbValue)
        {
            GivenAService();
            var actualDbValue = _service.GetLogarithmicScale(linearValue);
            Assert.AreEqual(expectedDbValue, actualDbValue, 0.0001f);
        }

        private void GivenAService()
        {
            _service = new AudioMixerService(_repository, _wrapper);
        }

        private void GivenMasterVolumeInRepository(float testValue) => _settings.MasterVolume.Returns(testValue);

        private void GivenAMusicValueInRepository(float testValue) => _settings.MusicVolume.Returns(testValue);

        private void GivenASfxValueInRepository(float testValue) => _settings.SfxVolume.Returns(testValue);

        private void GivenMutedStateInRepository(bool testValue) => _settings.IsMuted.Returns(testValue);
        
        private void WhenInitializeMasterVolumeIsCalled() => _service.InitializeMasterVolume();

        private void WhenInitializeSfxVolumeIsCalled() => _service.InitializeSfxVolume();

        private void WhenInitializeMusicVolumeIsCalled() => _service.InitializeMusicVolume();

        private void WhenInitializeToggleMuteIsCalled() => _service.InitializeToggleMute();
        
        private void WhenSetMusicVolumeIsCalled(float testValue) => _service.SetMusicVolume(testValue);

        private void WhenSetSfxVolumeIsCalled(float testValue) => _service.SetSfxVolume(testValue);

        private void WhenSetMasterVolumeIsCalled(float expectedVolume) => _service.SetMasterVolume(expectedVolume);

        private void ThenMasterVolumeIsApplied(string group, float value) => _wrapper.Received(1).SetFloat(group, value);

        private void ThenSfxVolumeIsApplied(string sfx, float expectedDbValue)
        {
            _wrapper.Received(1).SetFloat(sfx, expectedDbValue);
        }
        
        private void ThenMusicVolumeIsApplied(string music, float expectedDbValue)
        {
            _wrapper.Received(1).SetFloat("Music", expectedDbValue);
        }
        
        private void ThenMuteStateIsSetOn(string master, float expectedState)
        {
            _wrapper.Received(1).SetFloat("Master", expectedState);
        }
        
        private void ThenMuteStateIsSetOff(float expectedDbValue)
        {
            _wrapper.Received(1).SetFloat("Master", expectedDbValue);
        }
    }
}
