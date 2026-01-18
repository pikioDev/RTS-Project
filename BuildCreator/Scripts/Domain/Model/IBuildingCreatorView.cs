using System;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.BuildCreator.Scripts.Domain.Model
{
    public interface IBuildingCreatorView
    {
        event Action OnBuildingMade;    
        event Action<Vector3> OnMouseMoved; 
        event Action<Vector3> OnTryBuild;
        
        void UpdateGhost(Vector3 position, bool isValid);
        void TurnOffGhost();
        void RequestBuildOnServer(Vector3 position, BuildingType buildingType);
        Team GetTeam();
        void SetTeam(Team team);
    }
}