using System;
using Xenocode.Features.SuppliesManager.Scripts.Domain.Model;
using Xenocode.Features.SuppliesManager.Settings;
using Xenocode.Features.Unit.Scripts.Domain.Infrastructure;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.SuppliesManager.Scripts.Domain.Service
{
    public class SuppliesManagerService : ISuppliesService
    {
        private readonly SuppliesSettings _settings;
        private float _timer;

        public event Action<int> OnIncomeByTimer;
        public event Action<ulong, int> OnIncomeByKill;
        public SuppliesSettings GetSettings() => _settings;

        public SuppliesManagerService(MatchSettingsSO so)
        {
            _settings = new SuppliesSettings(so);
            _timer = _settings.IncomeTime;
        }
        
        public void Tick(float deltaTime)
        {
            _timer -= deltaTime;
            if (_timer <= 0f)
            {
                _timer = _settings.IncomeTime;
                OnIncomeByTimer?.Invoke(_settings.GoldPerIncome);
            }
        }
        
        public void HandleIncomeByKill(ulong killerClientId, int  reward)
        {
            OnIncomeByKill?.Invoke(killerClientId, reward);
        }
    }
}