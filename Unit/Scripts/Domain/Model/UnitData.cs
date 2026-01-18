namespace Xenocode.Features.Unit.Scripts.Domain.Model
{
    public struct UnitData
    {
        public int Health;
        public UnitProfile Profile;

        public UnitData(UnitProfile profile)
        {
            Health = profile.MaxHealth;
            Profile = profile;
        }
    }
}