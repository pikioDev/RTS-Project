using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "NewBuildingProfile", menuName = "Xenocode/Buildings/Building Profile")]
    public class BuildingProfileSo : ScriptableObject
    {
        [Header("Building Configuration")]
        [SerializeField] private BuildingType _buildingType;
        [SerializeField] private GameObject _buildingPrefab;
        [SerializeField] private UnitProfile _unitToSpawn;
        [SerializeField] private AudioProfile _audioProfile;
        [SerializeField] private string name = "default name";
        [SerializeField] private int goldCost = 100;
        [SerializeField] private int _spawnInterval = 20;
        [SerializeField] private int hitPoints = 900;
        [SerializeField] private string description = "default description";
        [SerializeField] private Texture2D _buildingTexture;
        
        public BuildingType BuildingType() => _buildingType;
        public UnitProfile GetUnitProfile() => _unitToSpawn;
        public AudioProfile GetAudioProfile() => _audioProfile;
        public GameObject GetBuildingPrefab() => _buildingPrefab;
        public string GetName() => name;
        public int GetCost() => goldCost;
        public int GetSpawnInterval() => _spawnInterval;
        public int GetMaxHitPoints() => hitPoints;
        public string GetDescription() => description;
        public Texture2D GetBuildingTexture() => _buildingTexture;
    }
}