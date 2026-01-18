using Xenocode.Features.PlayerPref.Scripts.Domain.Model;
using Xenocode.Features.SoundSettings.Scripts.Domain.Model;
using PlayerSettings = Xenocode.Features.PlayerPref.Scripts.Domain.Model.PlayerSettings;

namespace Xenocode.Features.SoundSettings.Scripts.Domain.Presenter
{
    public class SoundSettingsPresenter
    {
        private readonly ISoundSettingsView _view;
        private readonly IAudioMixerService _audioMixerService;

        public SoundSettingsPresenter(ISoundSettingsView view, IAudioMixerService audioMixerService)
        {
            _view = view;
            _audioMixerService = audioMixerService;
            SubscribeToViewEvents();
            InitializeView();
            InitializeAudioMixer();
        }
        
        private void SubscribeToViewEvents()
        {
            _view.OnMasterSliderChanged().AddListener(HandleMasterSliderChanged);
            _view.OnSfxSliderChanged().AddListener(HandleSfxSliderChanged);
            _view.OnMusicSliderChanged().AddListener(HandleMusicSliderChanged);
            _view.OnMuteToggleChanged().AddListener(HandleMuteToggleChanged);
        }
        
        private void InitializeView()
        {
            _view.SetMasterSlider(_audioMixerService.GetMasterVolume());
            _view.SetSfxSlider(_audioMixerService.GetSfxVolume());
            _view.SetMusicSlider(_audioMixerService.GetMusicVolume());
            _view.SetMuteToggle(_audioMixerService.GetMutedState());
        }
        
        private void InitializeAudioMixer()
        {
            _audioMixerService.InitializeMasterVolume();
            _audioMixerService.InitializeSfxVolume();
            _audioMixerService.InitializeMusicVolume();
            _audioMixerService.InitializeToggleMute();
        }
        
        private void HandleMasterSliderChanged(float volume) => _audioMixerService.SetMasterVolume(volume);

        private void HandleSfxSliderChanged(float volume) => _audioMixerService.SetSfxVolume(volume);

        private void HandleMusicSliderChanged(float volume) => _audioMixerService.SetMusicVolume(volume);

        private void HandleMuteToggleChanged(bool isMuted) => _audioMixerService.SetToggleMute(isMuted);
    }
}