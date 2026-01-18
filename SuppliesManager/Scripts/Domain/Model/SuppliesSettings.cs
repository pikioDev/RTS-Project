using Xenocode.Features.SuppliesManager.Settings;

namespace Xenocode.Features.SuppliesManager.Scripts.Domain.Model
{
    public struct SuppliesSettings
    {
        public int InitialGoldAmount;
        public int GoldPerIncome;
        public float IncomeTime;

        public SuppliesSettings(MatchSettingsSO so)
        {
            InitialGoldAmount = so.InitialGold;
            GoldPerIncome = so.GoldPerIncome;
            IncomeTime = so.IncomeTime;
        }
    }
}