using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;
using Xenocode.Features.Strike.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "New Unit Profile", menuName = "Xenocode/Units/Unit Profile")]
    public class UnitProfile : ScriptableObject
    {
        [Header("Info")]
        public string UnitName = "Unit";
        public UnitType UnitType;
        public StrikeData StrikeData; //Esta clase encapsularia los demas stast de ataque. 
        public AudioProfile AudioProfile;
        public GameObject Prefab;
        public Sprite Preview;

        [Header("Stats")]
        public int MaxHealth;
        public float MoveSpeed;
        public int GoldReward;

        public void SetUnitType(UnitType newType) =>  UnitType = newType;
        public UnitType GetUnitType() => UnitType;
    }
}
public enum AttackType { Melee, Range }