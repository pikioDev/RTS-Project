namespace Xenocode.Features.PlayerPref.Scripts.Domain.Model
{
    public interface IPlayerPrefsService
    {
        void SaveFloat(string key, float value);
        void SaveInt(string key, int value);
        void SaveBool(string key, bool value);
        float LoadFloat(string key, float defaultValue = 1f);
        int LoadInt(string key, int defaultValue = 0);
        bool LoadBool(string key, bool defaultValue = false);
        bool HasKey(string key);
        void Save();

    }
}