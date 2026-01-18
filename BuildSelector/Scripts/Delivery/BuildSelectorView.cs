using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Delivery;
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Provider;

namespace Xenocode.Features.BuildSelector.Scripts.Delivery
{
    public class BuildSelectorView : MonoBehaviour, IBuildSelectorView
    {
        [SerializeField] private BuildButton[] _buildButtons;
        
        void Start()
        {
            var networkObject = GetComponentInParent<NetworkObject>();
            
            if (!networkObject.IsOwner)
            {
                gameObject.SetActive(false);
                return; 
            }
            
            BuildSelectorProvider.Present(this);
        }

        public BuildButton[] GetBuildButtons()
        {
            return _buildButtons;
        }
    }
}