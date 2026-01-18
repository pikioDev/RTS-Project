namespace Xenocode.Features.PlayerPref.Scripts.Domain.Model
{
    public interface ISettingsRepository
    {
        public PlayerSettings GetSettings();
        public void SaveAllSettings();
    }
}