using Xenocode.Features.AttackStrategy.Scripts.Domain.Model;

namespace Xenocode.Features.AttackStrategy.Scripts.Domain.Providers
{
    public static class AttackStrategyProvider
    {
        public static IAttackStrategy GetAttackStrategy(AttackType type)
        {
            switch (type)
            {
                case AttackType.Range:
                    return new RangeStrategy();
                case AttackType.Melee:
                    return new MeleeStrategy();
                default:
                    return null;
            }
        }
    }
}