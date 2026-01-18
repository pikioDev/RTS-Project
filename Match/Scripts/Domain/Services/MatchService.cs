using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Xenocode.Features.Building.Scripts.Delivery;
using Xenocode.Features.Match.Scripts.Domain.Infrastructure;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Match.Scripts.Domain.Services
{
    public class MatchService
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly Dictionary<Team, CastleController> _castleRegistry = new();

        public event Action<CastleController> OnCastlesRegistered;

        public MatchService(ITeamsRepository teamsRepository, IPlayerRepository playerRepository)
        {
            _teamsRepository = teamsRepository;
            _playerRepository = playerRepository;
        }

        public void SetTeams()
        {
            var clientIds = NetworkManager.Singleton.ConnectedClientsIds;
            var shuffledClientIds = clientIds.OrderBy(_ => Guid.NewGuid()).ToList();
            var value = false;
            Dictionary<Team, List<ulong>> result = new();
            foreach (var id in clientIds)
            {

                var teamIndex = value ? 0 : 1;
                if (result.TryGetValue((Team)teamIndex, out var playerIds))
                    playerIds.Add(id);
                else
                    result[(Team)teamIndex] = new List<ulong> {id};
                Debug.Log("[MATCH SERVICE] -- Player: " + id + " Team: " + teamIndex);
                value = !value;
            }
            _teamsRepository.Set(result);
        }

        public Dictionary<Team, List<ulong>> GetTeams() => _teamsRepository.GetTeams();

        public Team GetTeam(ulong clientId) => _teamsRepository.GetTeams()
            .FirstOrDefault(teamKvp => teamKvp.Value.Contains(clientId))
            .Key;
        
        public IPlayerRepository GetPlayerRepository() => _playerRepository;
        
        public void RegisterCastle(Team team, CastleController castle)
        {
            _castleRegistry[team] = castle;
            OnCastlesRegistered?.Invoke(castle);
        }

        public CastleController GetCastle(Team team) => _castleRegistry.GetValueOrDefault(team);

        public Dictionary<Team, CastleController> GetAllCastles() => _castleRegistry;
    } 
}