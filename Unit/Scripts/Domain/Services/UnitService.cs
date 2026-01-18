using System;
using Xenocode.Features.Unit.Scripts.Domain.Infrastructure;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitProfileRepository _unitProfileRepository;
        private readonly IUnitsRepository _unitsRepository;

        public UnitService(IUnitProfileRepository unitProfileRepository, IUnitsRepository unitsRepository)
        {
            _unitProfileRepository = unitProfileRepository;
            _unitsRepository = unitsRepository;
        }
        
        public void AddToUnitsRepository(Guid unitIdentifier, UnitProfile profile)
        {
            _unitsRepository.Add(unitIdentifier, new UnitData(profile));
        }

        public void UpdateUnitHealth(int damageAmount, Guid guid)
        {
            var result = _unitsRepository.Get(guid).Health - damageAmount;
            _unitsRepository.UpdateHealth(guid, result);
        }

        public int GetUnitCurrentHealth(Guid guid) => _unitsRepository.Get(guid).Health;
        public UnitData GetUnitData(Guid guid) => _unitsRepository.Get(guid);
        public void RemoveUnit(Guid guid)
        {
            _unitsRepository.Remove(guid);
        }

        public UnitProfile GetUnitProfileByUnitType(UnitType unitType) => _unitProfileRepository.Get(unitType);
        
        public bool IsUnitRegistered(Guid guid) => _unitsRepository.Exists(guid);
    }
}