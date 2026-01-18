using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Unit.Scripts.Delivery;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.FieldOfView.Scripts
{
    public class FieldOfViewAgent : MonoBehaviour
    {
        public float ViewRadius = 20f;
        [Range(1, 360)] public float ViewAngle = 90f;
        public LayerMask TargetLayer;
        public LayerMask ObstacleLayer;
        
        public Transform PotentialTarget { get; set; } 

        private IFovTarget _agent;
        public IFovTarget Agent() => _agent;

        void Awake()
        {
            _agent = GetComponent<IFovTarget>();
        }

        void Start()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            FieldOfViewJobManager.Instance?.RegisterAgent(this);
        }

        public void OnDisable()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            FieldOfViewJobManager.Instance?.UnregisterAgent(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, ViewRadius);
            Vector3 fovLine1 = Quaternion.AngleAxis(ViewAngle / 2, transform.up) * transform.forward * ViewRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(-ViewAngle / 2, transform.up) * transform.forward * ViewRadius;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);
            if (PotentialTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, PotentialTarget.position);
            }
        }
    }
}