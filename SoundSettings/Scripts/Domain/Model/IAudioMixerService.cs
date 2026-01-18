namespace Xenocode.Features.SoundSettings.Scripts.Domain.Model
{
    public interface IAudioMixerService
    {
        public void InitializeMasterVolume();
        public void InitializeSfxVolume();
        public void InitializeMusicVolume();
        public void InitializeToggleMute();
        float GetMasterVolume();
        float GetSfxVolume();
        float GetMusicVolume();
        bool GetMutedState();
        void SetMasterVolume(float volume);
        void SetSfxVolume(float volume);
        void SetMusicVolume(float volume);
        void SetToggleMute(bool isMuted);
        float GetLogarithmicScale(float linearValue);
    }
}