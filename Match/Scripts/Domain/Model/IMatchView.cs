using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Xenocode.Features.Teams.Scripts.Domain.Model;
using Xenocode.Features.UserSupplies.Scripts.Delivery;

namespace Xenocode.Features.Match.Scripts.Domain.Model
{
    public interface IMatchView
    {
        UnityEvent OnAppear();
        UniTask ShowCountdown();
        Dictionary<ulong, UserSuppliesNet> CreatePlayers(Dictionary<ulong, Team> clientToTeamMap);
        void CreateTeamBase(Vector3 getCastleSpawnPoint, Team team);
        void SetCameraInitialPosition(Vector3 castleSpawnPoint);
        void CreateSuppliesManager();
    }
}
