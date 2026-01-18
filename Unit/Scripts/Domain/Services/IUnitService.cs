using System;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Services
{
    public interface IUnitService
    {
        UnitProfile GetUnitProfileByUnitType(UnitType unitType);
        void AddToUnitsRepository(Guid unitIdentifier, UnitProfile profile);
        void UpdateUnitHealth(int damageAmount, Guid guid);
        int GetUnitCurrentHealth(Guid guid);
        UnitData GetUnitData(Guid guid);
        void RemoveUnit(Guid guid);
        bool IsUnitRegistered(Guid guid);
    }
}