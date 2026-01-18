using Xenocode.Features.UserSupplies.Scripts.Delivery;
using Xenocode.Features.UserSupplies.Scripts.Domain.Model;

namespace Xenocode.Features.UserSupplies.Scripts.Domain.Presentation
{
    public class UserSuppliesPresenter
    {
        private readonly IUserSuppliesView _view;
        private readonly UserSuppliesNet _user;
        private readonly IUserSuppliesService _userSuppliesService;
        
        public UserSuppliesPresenter(IUserSuppliesView view, UserSuppliesNet user,
            IUserSuppliesService userSuppliesService)
        {
            _view = view;
            _user = user;
            
            _user.CurrentGold.OnValueChanged += HandleGoldChanged;
            _userSuppliesService = userSuppliesService;
            
            SubscribeToNetObjEvents();
            SubscribeToServiceEvents();
            InitializeView();
        }

        private void SubscribeToServiceEvents()
        {
            _userSuppliesService.OnInsufficientFounds += HandleFoundsNotification;
        }

        private void HandleFoundsNotification()
        {
            _view.NotifyInsufficientFounds();
        }

        private void HandleGoldChanged(int oldVal, int newVal)
        {
            _view.UpdateGoldText(newVal);
            
            if (newVal > oldVal)
            {
                _view.UpdateNotificationText(newVal - oldVal);
            }
        }

        private void InitializeView()
        {
            _view.UpdateGoldText(_user.CurrentGold.Value);
        }

        private void SubscribeToNetObjEvents()
        {
            _user.OnIncomeNotification += HandleNotification;
            _user.OnGoldValueUpdated += HandleGoldText;
        }

        private void HandleGoldText(int newValue)
        {
            _view.UpdateGoldText(newValue);
        }

        private void HandleNotification(int newValue)
        {
            _view.UpdateNotificationText(newValue);
        }
    }
}