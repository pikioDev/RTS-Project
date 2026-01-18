using Unity.Netcode;
using UnityEngine;

namespace Xenocode.Features.FieldOfView.Scripts
{
    public class FieldOfViewTarget : MonoBehaviour
    {
        private IFovTarget _target;
        public IFovTarget Target() => _target;
        
        private void Awake()
        {
            _target = GetComponent<IFovTarget>();
        }
        
        void Start()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            FieldOfViewJobManager.Instance?.RegisterTarget(this);
        }

        void OnDisable()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            FieldOfViewJobManager.Instance?.UnregisterTarget(this);
        }
    }
}