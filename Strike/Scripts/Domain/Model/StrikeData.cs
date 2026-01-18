using UnityEngine;

namespace Xenocode.Features.Strike.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "NewStrikeData", menuName = "Xenocode/Strikes/Strike Data")]
    public class StrikeData : ScriptableObject  
    {
        [Header("Info")]
        public StrikeType StrikeType;
        public AttackType AttackType;

        [Header("Stats")] 
        public float Range = 2f;
        public float Cooldown = 1;
        public int Damage = 25;
        public float Lifetime = 1f;
    }
}