using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Xenocode.Features.AttackStrategy.Scripts.Domain.Providers;
using Xenocode.Features.Audio.Scripts.Domain.Provider;
using Xenocode.Features.BehaviorTree.Scripts.Domain;
using Xenocode.Features.FSM.Domain.Providers;
using Xenocode.Features.Match.Scripts.Delivery;
using Xenocode.Features.Match.Scripts.Domain.Providers;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Provider;
using Xenocode.Features.Teams.Scripts.Domain.Providers;
using Xenocode.Features.Unit.Scripts.Domain.Infrastructure;
using Xenocode.Features.Unit.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Presentation;
using Xenocode.Features.Unit.Scripts.Domain.Services;
using Xenocode.Features.Waypoints.Scripts.Domain.Providers;

namespace Xenocode.Features.Unit.Scripts.Domain.Providers
{
    public static class UnitProvider
    {
        private static UnitService _unitService;
        private static UnitProfileRepository _unitProfileRepository;
        private static UnitsRepository _unitsRepository;

        public static void Present(INetworkUnit network ,ulong ownerClientId, Guid guid, UnitType unitType,
            NavMeshAgent agent, Opsive.BehaviorDesigner.Runtime.BehaviorTree behaviorTree, UnitEvents unitEvents)
        {
            var unitService = GetUnitService();
            var profile = unitService.GetUnitProfileByUnitType(unitType);
            var attackStrategy = AttackStrategyProvider.GetAttackStrategy(profile.StrikeData.AttackType);
            var stateMachine = FSMProvider.GetUnitStateMachine(profile, agent, attackStrategy);
            var suppliesService = SuppliesManagerProvider.GetService();
            var audioService = AudioProvider.GetAudioEvents();
            new UnitPresenter(network,ownerClientId, WaypointsProvider.GetWaypointsService(),
                MatchProvider.GetMatchService(), TeamProvider.GetTeamService(), unitService, guid,
                stateMachine, behaviorTree,unitEvents, suppliesService, audioService);
        }

        public static IUnitService GetUnitService() =>
            _unitService ??= new UnitService(GetUnitProfileRepository(), GetUnitsRepository());

        public static IUnitProfileRepository GetUnitProfileRepository() =>
            _unitProfileRepository ??= new UnitProfileRepository();
        
        private static IUnitsRepository GetUnitsRepository() => _unitsRepository ??= new UnitsRepository();
    }
}