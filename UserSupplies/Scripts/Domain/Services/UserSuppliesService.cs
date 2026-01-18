using System;
using Xenocode.Features.UserSupplies.Scripts.Delivery;
using Xenocode.Features.UserSupplies.Scripts.Domain.Model;

namespace Xenocode.Features.UserSupplies.Scripts.Domain.Services
{
    public class UserSuppliesService : IUserSuppliesService
    {
        private UserSuppliesNet _localUser;
        
        public event Action OnInsufficientFounds;
        
        public int GetCurrentGold() => _localUser.CurrentGold.Value;
        
        public void Initialize(UserSuppliesNet user)
        {
            _localUser = user;
        }

        
        
        public void NotifyInsufficientFounds() => OnInsufficientFounds?.Invoke();
    }
}