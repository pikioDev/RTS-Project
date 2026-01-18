using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Infrastructure
{
    public class UnitProfileRepository : IUnitProfileRepository
    {
        private List<UnitProfile> _unitProfiles;
        public UnitProfileRepository()
        {
            FillProfiles();
        }

        private void FillProfiles()
        {
            _unitProfiles = Resources.Load<UnitProfileList>("UnitsProfileList").Profiles;
        }

        public UnitProfile Get(UnitType unitType) => 
            _unitProfiles.First(profile => profile.GetUnitType() == unitType);
    }
}