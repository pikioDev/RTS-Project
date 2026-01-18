using Xenocode.Features.Match.Scripts.Domain.Services;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Model;

namespace Xenocode.Features.SuppliesManager.Scripts.Domain.Presentation
{
    public class SuppliesManagerPresenter
    {
        private readonly ISuppliesManager _manager;
        private readonly ISuppliesService _suppliesService;
        private readonly MatchService _matchService;
        private readonly SuppliesSettings _config;

        public SuppliesManagerPresenter(ISuppliesManager manager, ISuppliesService suppliesService,
            MatchService matchService)
        {
            _manager = manager;
            _suppliesService = suppliesService;
            _matchService = matchService;
            SubscribeToViewEvents();
            SubscribeToService();

            InitializeSupplies();
        }

        private void SubscribeToViewEvents()
        {
            _manager.OnServerTick += HandleServerTick;
        }
        
        private void SubscribeToService()
        {
            _suppliesService.OnIncomeByTimer += HandleGoldIncome;
            _suppliesService.OnIncomeByKill += HandleKillReward;
        }

        private void HandleKillReward(ulong killerId, int reward)
        {
            var killerEntity = _matchService.GetPlayerRepository().GetPlayer(killerId);
            killerEntity.AddGold(reward);
            killerEntity.NotifyKillRewardClientRpc(reward);
        }
        private void HandleServerTick(float deltaTime)
        {
            _suppliesService.Tick(deltaTime);
        }
        
        private void HandleGoldIncome(int income)
        {
            foreach (var entity in _matchService.GetPlayerRepository().GetAllSuppliesEntities())
            {
                entity.AddGold(income);
            }
        }
        
        private void InitializeSupplies()
        {
            foreach (var entity in _matchService.GetPlayerRepository().GetAllSuppliesEntities())
            {
                entity.AddGold(_suppliesService.GetSettings().InitialGoldAmount);
            }
        }
    }
}