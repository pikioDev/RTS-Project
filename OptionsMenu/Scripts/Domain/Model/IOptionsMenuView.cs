using UnityEngine.Events;

namespace Xenocode.Features.OptionsMenu.Domain.Model
{
    public interface IOptionsMenuView
    {
        UnityEvent OnScreenOptionsPressed();
        UnityEvent OnAudioOptionsPressed();
        void ShowScreenOptions();
        void ShowAudioOptions();
    }
}