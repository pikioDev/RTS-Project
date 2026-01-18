using Xenocode.Features.OptionsMenu.Domain.Model;
using Xenocode.Features.OptionsMenu.Domain.Presentation;

namespace Xenocode.Features.OptionsMenu.Domain.Providers
{
    public static class OptionsMenuProvider
    {
        public static void Present(IOptionsMenuView view) =>
            new OptionsMenuPresenter(view);
    }
}