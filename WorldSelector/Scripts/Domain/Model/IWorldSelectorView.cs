using System;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.WorldSelector.Scripts.Domain.Model
{
    public interface IWorldSelectorView
    {
        event Action<INetworkUnit> OnUnitClicked;

        event Action<IBuildingNetView> OnBuildingClicked;
    
        event Action<Vector3> OnTerrainClicked;
        
        event Action OnCancelClick;
    }
}