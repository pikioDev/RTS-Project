using System;
using UnityEngine;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Building.Scripts.Domain.Model
{
    public interface IBuildingNetView
    {
        ulong GetOwnerClientId();
        
        public event Action OnServerUpdateTimer;
        
        public event Action<float> OnTimerRemainingChanged;
        
        public event Action<int> OnHitsPointsChanged;
        
        int GetTeamIndex();
        
        void SetTeam(Team playerTeamValue);
        
        BuildingProfileSo GetBuildingProfile();
        void SetTimerRemaining(float currentTime);
        float GetTimerRemaining();
        void SetHitsPointsNet(int getHitPoints);
        int GetHitPoints();
        void SetBaseMapSprite();
        bool IsAlly();
        Vector3 GetPosition();
    }
}