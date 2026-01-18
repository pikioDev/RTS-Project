namespace Xenocode.Features.UserSupplies.Scripts.Domain.Model
{
    public interface IUserSuppliesView
    {
        void UpdateGoldText(int amount);
        
        public void UpdateNotificationText(int amount);
        void NotifyInsufficientFounds();
    }
}