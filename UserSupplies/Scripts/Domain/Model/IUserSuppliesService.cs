using System;
using Xenocode.Features.UserSupplies.Scripts.Delivery;

namespace Xenocode.Features.UserSupplies.Scripts.Domain.Model
{
    public interface IUserSuppliesService
    {
        int GetCurrentGold();
        
        void Initialize(UserSuppliesNet user);
        

        event Action OnInsufficientFounds;
        
        public void NotifyInsufficientFounds();
    }
}