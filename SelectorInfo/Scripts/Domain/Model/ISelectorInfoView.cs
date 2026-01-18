using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.SelectorInfo.Scripts.Domain.Model
{
    public interface ISelectorInfoView
    {
        void HideInfo();
        
        void ShowSpawnTimer(string buildingProfile);
        
        void ShowBuildingHitPoints(string hpFormatted);
        
        void ShowUnitHitPoints(string hp);
        
        void ShowDescription(string description);
        
        void ShowUnitStats(UnitProfile profile);
    }
}