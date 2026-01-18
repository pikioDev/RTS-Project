using System;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Unit.Scripts.Delivery;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Unit.Scripts.Domain.Model
{
    public interface INetworkUnit
    {
        event Action<Guid, int> OnUnitHitPointsChanged;
        Guid GetGuid();
        void SetGuid(Guid guid);
        Team GetTeam();
        void SetTeam(int team);
        NetworkObject GetNetworkObject();
        UnitView GetView();
        UnitType GetUnitType();
        void SetUnitType(UnitType unitType);
        void ShowHighLight();
        void HideHighLight();
        void ReturnToPool();
        void DisplayDeathClient();
        int GetCurrentNetworkHealth();
        void SetNetworkHealth(int health);
        void PerformAttackClient(Vector3 target, float lifeTime, int strikeType);
        Vector3 GetPosition();
        bool IsAlly();
        void UpdateMovementSpeedClientRpc(float speed = 0);
        void SetTeamColorClientRpc(Color getTeamColor);
    }
}
