using System.Collections.Generic;
using Unity.Netcode;
using Xenocode.Features.BuildCreator.Scripts.Domain.Provider;
using Xenocode.Features.Match.Scripts.Domain.Model;
using Xenocode.Features.Match.Scripts.Domain.Services;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.Match.Scripts.Domain.Presentation
{
    public class MatchPresenter
    {
        private readonly IMatchView _view;
        private readonly MatchService _matchService;

        public MatchPresenter(IMatchView view, MatchService matchService)
        {
            _view = view;
            _matchService = matchService;
            SubscribeToViewEvents();
        }

        private void SubscribeToViewEvents()
        {
            _view.OnAppear().AddListener(HandleOnAppear);
        }

        private async void HandleOnAppear()
        {
            CreateTeams();
            CreateTeamsBases();
            await _view.ShowCountdown();
            CreatePlayers();
            SetCameraInitialPosition();
            CreateSuppliesManager();
        }

        private void CreateSuppliesManager()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _view.CreateSuppliesManager();
        }

        private void SetCameraInitialPosition()
        {
            _view.SetCameraInitialPosition(BuildingCreatorProvider.GetPlacementService().GetCastleSpawnPoint(GetMyTeam()));
        }

        private Team GetMyTeam() => _matchService.GetTeam(NetworkManager.Singleton.LocalClientId);

        private void CreateTeamsBases()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            var teams = _matchService.GetTeams();
            foreach (var team in teams)
            {
                _view.CreateTeamBase(BuildingCreatorProvider.GetPlacementService().GetCastleSpawnPoint(team.Key), team.Key);
            }
        }

        private void CreateTeams()
        {
            _matchService.SetTeams();
        }
        
        private void CreatePlayers()
        {
            if (!NetworkManager.Singleton.IsServer) return;
            var entities = _view.CreatePlayers(GetTeamMap());
            foreach (var kvp in entities)
            {
                _matchService.GetPlayerRepository().Add(kvp.Key, kvp.Value);
            }
        }

        private Dictionary<ulong, Team> GetTeamMap()
        {
            var teams = _matchService.GetTeams();
            var clientToTeamMap = new Dictionary<ulong, Team>();
            foreach (var teamEntry in teams)
            {
                var team = teamEntry.Key;
                foreach (var clientId in teamEntry.Value)
                {
                    clientToTeamMap[clientId] = team;
                }
            }
            return clientToTeamMap;
        }
    }
}