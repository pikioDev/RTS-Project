using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Match.Scripts.Delivery
{
    public class UnitEvents : MonoBehaviour
    {
        public UnityEvent<int, ulong> OnDamageTaken { get; } = new();
        public UnityEvent<Vector3, float> OnAttack { get; } = new();
        public UnityEvent<UnitType> OnTypeChange { get; } = new();
        public UnityEvent<bool> OnReachTarget { get; } = new();
    }
}
