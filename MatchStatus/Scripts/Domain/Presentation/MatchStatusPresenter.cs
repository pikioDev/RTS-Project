using Xenocode.Features.Building.Scripts.Delivery;
using Xenocode.Features.Match.Scripts.Domain.Services;
using Xenocode.Features.MatchStatus.Scripts.Delivery;
using Xenocode.Features.MatchStatus.Scripts.Domain.Model;
using Xenocode.Features.Teams.Scripts.Domain.Model;

namespace Xenocode.Features.MatchStatus.Scripts.Domain.Presentation
{
    public class MatchStatusPresenter
    {
        private readonly IMatchStatusView _view;
        private readonly MatchService _matchService;

        public MatchStatusPresenter(MatchStatusView view, MatchService matchService)
        {
            _view = view;
            _matchService = matchService;

            _matchService.OnCastlesRegistered += InitializeView;
        }

        private void InitializeView(CastleController castle)
        {
            castle.CurrentHealth.OnValueChanged += UpdateAllHealthVisuals;
            UpdateAllHealthVisuals(0, 0);
        }

        private void UpdateAllHealthVisuals(int old, int income)
        {
            var castles = _matchService.GetAllCastles();
            if (castles.Count < 2) return;

            var castleA = castles[(Team)0];
            var castleB = castles[(Team)1];
            
            _view.UpdateIndividualBar(0, castleA.GetCurrentHealth(), castleA.GetMaxHealth());
            _view.UpdateIndividualBar(1, castleB.GetCurrentHealth(), castleB.GetMaxHealth());
            
            _view.UpdateSharedBar(castleB.GetCurrentHealth(), castleA.GetCurrentHealth());
        }
    }
}