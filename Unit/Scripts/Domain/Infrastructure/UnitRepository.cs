using System;
using System.Collections.Generic;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Infrastructure
{
    public class UnitsRepository : IUnitsRepository
    {
        private readonly Dictionary<Guid, UnitData> _units = new();
        private readonly Dictionary<ulong, Guid> _networkIdToGuid = new();
        public void Add(Guid guid, UnitData unitData)
        {
            _units.Add(guid, unitData);
        }

        public UnitData Get(Guid guid) => _units[guid];
        
        public void UpdateHealth(Guid guid, int result)
        {
            var currentUnit = _units[guid];
            currentUnit.Health = result;
            _units[guid] = currentUnit;
        }

        public void Remove(Guid guid)
        {
            if (_units.TryGetValue(guid, out var unitData))
            {
                _units.Remove(guid);
            }
        }
        
        public bool Exists(Guid guid) => _units.ContainsKey(guid);
    }
}