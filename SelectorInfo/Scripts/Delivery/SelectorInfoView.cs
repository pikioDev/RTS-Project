using System;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Model;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Provider;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.SelectorInfo.Scripts.Delivery
{
    public class SelectorInfoView : MonoBehaviour, ISelectorInfoView
    {
        [SerializeField] TextMeshProUGUI _spawnIntervalOrMana;
        [SerializeField] TextMeshProUGUI _hitPoints;
        [SerializeField] TextMeshProUGUI _name;
        [SerializeField] TextMeshProUGUI _description;
        [SerializeField] TextMeshProUGUI _attackRange;
        [SerializeField] TextMeshProUGUI _movementSpeed;
        [SerializeField] Image _preview;
        
        void Start()
        {
            var networkObject = GetComponentInParent<NetworkObject>();
            
            if (!networkObject.IsOwner)
            {
                gameObject.SetActive(false);
                return; 
            }
            
            SelectorInfoProvider.Present(this);
            HideInfo();
        }
        
        public void HideInfo()
        {
            _hitPoints.text = String.Empty;
            _spawnIntervalOrMana.text = String.Empty;
            _name.text = String.Empty;
            _description.text = String.Empty;
            _attackRange.text = String.Empty;
            _movementSpeed.text = String.Empty;
            SetPreviewAlpha(0);
        }
        
        public void ShowSpawnTimer(string timer)
        {
            _spawnIntervalOrMana.text = timer;
        }

        public void ShowBuildingHitPoints(string hpFormatted)
        {
            _hitPoints.text = hpFormatted;
        }

        public void ShowUnitHitPoints(string hp)
        {
            _hitPoints.text = hp;
        }

        public void ShowDescription(string description)
        {
            _description.text = description;
        }

        public void ShowUnitStats(UnitProfile profile)
        {
            _attackRange.text = $"{profile.StrikeData.AttackType}: {profile.StrikeData.Range}({profile.StrikeData.Damage}dps)";
            _movementSpeed.text = $"Movement: {profile.MoveSpeed}";
            _name.text = profile.UnitName;
            ShowPreview(profile.Preview);
        }

        private void ShowPreview(Sprite profilePreview)
        {
            _preview.sprite = profilePreview;
            SetPreviewAlpha(0.8f);
        }

        private void SetPreviewAlpha(float a)
        {
            var color = _preview.color;
            color.a =  a;
            _preview.color = color;
        }
    }
}