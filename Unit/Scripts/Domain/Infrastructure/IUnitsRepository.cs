using System;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Infrastructure
{
    public interface IUnitsRepository
    {
        void Add(Guid unitIdentifier, UnitData unitData);
        UnitData Get(Guid guid);
        void UpdateHealth(Guid guid, int result);
        void Remove(Guid guid);
        public bool Exists(Guid guid);
    }
}