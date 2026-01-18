using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xenocode.Features.Building.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Delivery
{
    public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BuildingType _buildingType;
        public Action<BuildingType> OnButtonHighlighted;
        public Action OnButtonUnhighlighted;
        private Button _clickButton; 
        public Button GetButton() => _clickButton;
        public BuildingType GetBuildingType() => _buildingType;
        
        private void Awake()
        {
            _clickButton = GetComponent<Button>(); 
        }
        
        public void OnPointerEnter(PointerEventData eventData) => 
            OnButtonHighlighted?.Invoke(_buildingType);

        public void OnPointerExit(PointerEventData eventData) => 
            OnButtonUnhighlighted?.Invoke();
    }
}