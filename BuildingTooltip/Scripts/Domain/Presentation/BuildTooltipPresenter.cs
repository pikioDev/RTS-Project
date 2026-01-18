using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.BuildingTooltip.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;
using Xenocode.Features.SelectorInfo.Scripts.Domain.Model;

namespace Xenocode.Features.BuildingTooltip.Scripts.Domain.Presentation
{
    public class BuildTooltipPresenter
    {
        private readonly IBuildTooltipView _view;
        private readonly IBuildSelectorService _selectorService;
        private readonly ISelectorInfoService _selectorInfoService;

        public BuildTooltipPresenter(IBuildTooltipView view, IBuildSelectorService selectorService, ISelectorInfoService selectorInfoService)
        {
            _view = view;
            _selectorService = selectorService;
            _selectorInfoService = selectorInfoService;
            SubscribeToSelectorService();
            _view.HideTooltip();
        }

        private void SubscribeToSelectorService()
        {
            _selectorService.OnButtonHighlight += HandleHighlightedButton;
            _selectorService.OnButtonUnhighlight += HandleUnhighlightedButton;
        }

        private void HandleHighlightedButton(BuildingType type)
        {
            _view.ShowTooltip(type, _selectorInfoService.GetFocusedBuildingProfile(type));
        }
        
        private void HandleUnhighlightedButton()
        {
            _view.HideTooltip();
        }
    }
}