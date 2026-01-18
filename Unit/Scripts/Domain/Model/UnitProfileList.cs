using System.Collections.Generic;
using UnityEngine;

namespace Xenocode.Features.Unit.Scripts.Domain.Model
{
    [CreateAssetMenu(fileName = "New Unit Profile", menuName = "Xenocode/Units/Unit Profile List")]
    public class UnitProfileList : ScriptableObject
    {
        public List<UnitProfile> Profiles;
    }
}
