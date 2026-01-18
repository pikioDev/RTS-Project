using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Model;
using Xenocode.Features.BuildCreator.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;
using Xenocode.Features.UserSupplies.Scripts.Domain.Model;
using Xenocode.Features.WorldSelector.Scripts.Domain.Model;

namespace Xenocode.Features.BuildCreator.Scripts.Domain.Presentation
{
    public class BuildingCreatorPresenter
    {
        private readonly IBuildingCreatorView _view;
        private readonly IBuildingPlacementService _placementService;
        private readonly IBuildSelectorService _selectorService;
        private readonly IWorldSelectorService _worldSelectorService;
        private readonly IUserSuppliesService _userSuppliesService;
        private readonly IAudioEvents _audioService;


        public BuildingCreatorPresenter(IBuildingCreatorView view, IBuildingPlacementService placementService,
            IBuildSelectorService selectorService, IWorldSelectorService worldSelectorService,
            IUserSuppliesService userSuppliesService, IAudioEvents audioService)
        {
            _view = view;
            _placementService = placementService;
            _selectorService = selectorService;
            _worldSelectorService = worldSelectorService;
            _userSuppliesService = userSuppliesService;
            _audioService = audioService;
            
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _view.OnMouseMoved += HandleMouseMoved;
            _view.OnBuildingMade += HandleBuildingMade;
            _view.OnTryBuild += HandleTryBuild;
            _worldSelectorService.OnCancelClick += HandleCancelClick;
        }

        private void HandleCancelClick()
        {
            _view.TurnOffGhost();
            _selectorService.DeselectBuilding();
        }

        private void HandleMouseMoved(Vector3 rawPosition)
        {
            if (!_selectorService.IsButtonSelected()) return;

            var type = _selectorService.GetSelectedType();
            Vector3 snappedPos = _placementService.GetSnappedPosition(rawPosition, type);
            bool isValid = _placementService.IsValidPlacement(snappedPos, _view.GetTeam());

            _view.UpdateGhost(snappedPos, isValid);
        }
        
        private void HandleBuildingMade()
        {
            _view.TurnOffGhost();
            _selectorService.DeselectBuilding();
        }
        
        private void HandleTryBuild(Vector3 rawPosition)
        {
            if (!_selectorService.IsButtonSelected()) return;

            var type = _selectorService.GetSelectedType();
            Vector3 snappedPos = _placementService.GetSnappedPosition(rawPosition, type);

            if (!HasEnoughGold())
            {
                _userSuppliesService.NotifyInsufficientFounds();
                return;
            }
            
            if (_placementService.IsValidPlacement(snappedPos, _view.GetTeam()))
            {
                _view.RequestBuildOnServer(snappedPos, type);
                PlaySoundEffect(snappedPos);
            }
            
        }
        
        private bool HasEnoughGold()
        {
            var buildCost = _placementService.GetBuildData()
                .GetProfileByType(_selectorService.GetSelectedType())
                .GetCost();
            
            return buildCost <= _userSuppliesService.GetCurrentGold();
        }
        
        private void PlaySoundEffect(Vector3 position)
        {
            var profile = _placementService.GetBuildData()
                .GetProfileByType(_selectorService.GetSelectedType());
            
            _audioService.PlaySound(profile.GetAudioProfile().name, (int)AudioClipType.Spawn, position);
        }
    }
}