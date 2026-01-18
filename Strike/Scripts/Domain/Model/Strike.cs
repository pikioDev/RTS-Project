using UnityEngine;

namespace Xenocode.Features.Strike.Scripts.Domain.Model
{
    public abstract class Strike : MonoBehaviour, IStrike
    {
        [SerializeField] private StrikeType _strikeType;
        public StrikeType GetStrikeType() => _strikeType;
        public Transform GetTransform() => transform;
        
        public abstract void Execute(Transform pos, Vector3 target, float travelTime);

        public void SetStrikeType(StrikeType strikeType)
        {
            _strikeType = strikeType; 
        }
        public void SetPositionAndRotations(Transform spawnPoint)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        public void SetParent(Transform spawnPoint)
        {
            transform.parent = spawnPoint;
        }
    }
}
