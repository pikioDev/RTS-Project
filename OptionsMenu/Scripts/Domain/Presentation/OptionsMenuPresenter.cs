using Xenocode.Features.OptionsMenu.Domain.Model;

namespace Xenocode.Features.OptionsMenu.Domain.Presentation
{
    public class OptionsMenuPresenter
    {
        private readonly IOptionsMenuView _view;

        public OptionsMenuPresenter(IOptionsMenuView view)
        {
            _view = view;
            SubscribeToViewEvents();
        }

        private void SubscribeToViewEvents()
        {
            _view.OnScreenOptionsPressed().AddListener(HandleScreenOptionsPressed);
            _view.OnAudioOptionsPressed().AddListener(HandleAudioOptionsPressed);
        }

        private void HandleScreenOptionsPressed()
        {
            _view.ShowScreenOptions();
        }

        private void HandleAudioOptionsPressed()
        {
            _view.ShowAudioOptions();
        }
    }
}