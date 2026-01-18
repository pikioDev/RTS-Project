using UnityEngine;
using Xenocode.Features.PlayerPref.Scripts.Domain.Model;

namespace Xenocode.Features.PlayerPref.Scripts.Domain.Services
{
    public class PlayerPrefsService : IPlayerPrefsService
    {
        public void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
        public void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public void SaveBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

        public float LoadFloat(string key, float defaultValue = 1f) => PlayerPrefs.GetFloat(key, defaultValue);
        public int LoadInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);
        public bool LoadBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
        public void Save() => PlayerPrefs.Save();
    }
    
}