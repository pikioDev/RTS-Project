using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;


namespace Xenocode.Features.BuildSelector.Scripts.Domain.Presenter
{
    public class BuildSelectorPresenter
    {
        private readonly IBuildSelectorService _buildSelectorServices;
        private readonly IBuildSelectorView _view;
        public BuildSelectorPresenter(IBuildSelectorView view, IBuildSelectorService buildSelectorServices)
        {
            _view = view;
            _buildSelectorServices = buildSelectorServices;
            SubscribeToViewEvents();
           
        }

        private void SubscribeToViewEvents()
        {
            foreach (var buildButton in _view.GetBuildButtons())
            {
                buildButton.GetButton().onClick.AddListener(() => 
                    _buildSelectorServices.SelectBuilding(buildButton.GetBuildingType())); 
                
                buildButton.OnButtonHighlighted += HandleButtonHighlighted;
                buildButton.OnButtonUnhighlighted += HandleButtonUnhighlighted;
            }
        }

        private void HandleButtonHighlighted(BuildingType type)
        {
            _buildSelectorServices.HighlightButton(type);
        }
        
        private void HandleButtonUnhighlighted()
        {
            _buildSelectorServices.UnhighlightButton();
        }
    }
}