using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;

namespace Xenocode.Features.BuildCreator.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "Building Database", menuName = "Xenocode/Buildings/Building Profile List")]
    public class BuildingsDatabaseSO : ScriptableObject
    {
        public List<BuildingProfileSo> buildings;
        [System.NonSerialized] private Dictionary<BuildingType, BuildingProfileSo> _profileMap;
        
        public BuildingProfileSo GetProfileByType(BuildingType type)
        {
            if (_profileMap == null) Initialize();

            if (_profileMap.TryGetValue(type, out BuildingProfileSo profile))
            {
                return profile;
            }
            return null;
        }
        
        private void OnEnable()
        {
            Initialize(); 
        }

        public void Initialize()
        {
            if (_profileMap != null) return; 
            
            _profileMap = new Dictionary<BuildingType, BuildingProfileSo>();
            foreach (var profile in buildings)
            {
                _profileMap.Add(profile.BuildingType(), profile); 
            }
        }
    }
}