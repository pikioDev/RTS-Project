using Xenocode.Features.MainMenu.Scripts.Domain.Model;
using Xenocode.Features.MainMenu.Scripts.Domain.Presenter;

namespace Xenocode.Features.MainMenu.Scripts.Domain.Provider
{
    public static class MainMenuProvider
    {
        public static void Present(IMainMenuView view) => 
            new MainMenuPresenter(view);
        
    }
}