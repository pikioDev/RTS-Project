using System;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.SuppliesManager.Scripts.Domain.Model
{
    public interface ISuppliesService
    {
        event Action<int> OnIncomeByTimer;
        public event Action<ulong, int> OnIncomeByKill;
        void HandleIncomeByKill(ulong killerClientId, int reward);
        SuppliesSettings GetSettings();
        void Tick(float deltaTime);
    }
}