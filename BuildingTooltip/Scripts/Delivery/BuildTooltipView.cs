using TMPro;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.BuildingTooltip.Scripts.Domain.Model;
using Xenocode.Features.BuildingTooltip.Scripts.Domain.Provider;

namespace Xenocode.Features.BuildingTooltip.Scripts.Delivery
{
    public class BuildTooltipView : MonoBehaviour, IBuildTooltipView
    {
        [SerializeField] private TMP_Text _buildingName;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private TMP_Text _cooldown;
        [SerializeField] private TMP_Text _trainsName;
        [SerializeField] private TMP_Text _upgradeAmount;
        [SerializeField] private TMP_Text _unitName;
        [SerializeField] private TMP_Text _attack;
        [SerializeField] private TMP_Text _hitpoints;
        [SerializeField] private TMP_Text _abilities;
        

        void Start()
        { 
            var networkObject = GetComponentInParent<NetworkObject>();
            
            if (!networkObject.IsOwner)
            {
                gameObject.SetActive(false);
                return; 
            }

            BuildingTooltipProvider.Present(this);
        }

        public void ShowTooltip(BuildingType type, BuildingProfileSo profile)
        {
            UpdateUI(profile);
            SetCanvasEnabled();
        }

        private void UpdateUI(BuildingProfileSo profile)
        {
            var unit = profile.GetUnitProfile();
            _buildingName.text = profile.GetName();
            _cost.text = profile.GetCost().ToString();
            _cooldown.text = profile.GetSpawnInterval().ToString();
            _upgradeAmount.text = "0";
            _trainsName.text = unit.UnitName;
            _unitName.text = unit.UnitName;
            _attack.text = unit.StrikeData.AttackType + "(" + unit.StrikeData.Damage + "dps)";
            _hitpoints.text = profile.GetMaxHitPoints().ToString();
            _abilities.text = $"{profile.GetUnitProfile().StrikeData.StrikeType} {profile.GetUnitProfile().StrikeData.Damage}";
        }

        void SetCanvasEnabled() => gameObject.SetActive(true);

        public void HideTooltip() => gameObject.SetActive(false);
    }
}