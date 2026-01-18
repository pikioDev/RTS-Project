using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Model
{
    public interface IBuildingView
    {
        public void SpawnUnit(int team);
        BuildingProfileSo GetBuildingProfile();
    }
}