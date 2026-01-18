
using Xenocode.Features.BuildSelector.Scripts.Domain.Model;
using Xenocode.Features.BuildSelector.Scripts.Domain.Presenter;
using Xenocode.Features.BuildSelector.Scripts.Domain.Service;

namespace Xenocode.Features.BuildSelector.Scripts.Domain.Provider
{
    public static class BuildSelectorProvider
    {
        private static IBuildSelectorService _buildSelectorServices;

        public static void Present(IBuildSelectorView view) =>
            new BuildSelectorPresenter(view, GetBuildSelectorService());

        public static IBuildSelectorService GetBuildSelectorService() =>
            _buildSelectorServices ??= new BuildSelectorService();
    }
}