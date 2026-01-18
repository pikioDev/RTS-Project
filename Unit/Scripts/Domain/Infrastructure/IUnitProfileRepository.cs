using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Infrastructure
{
    public interface IUnitProfileRepository
    {
        UnitProfile Get(UnitType unitType);
    }
}