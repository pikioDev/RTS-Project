using UnityEngine;

namespace Xenocode.Features.Strike.Scripts.Domain.Model
{
    public interface IStrike
    {
        StrikeType GetStrikeType();
        Transform GetTransform();
        void SetStrikeType(StrikeType strikeType);
        void SetPositionAndRotations(Transform spawnPointPosition);
    }
}