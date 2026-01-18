using Xenocode.Features.UserSupplies.Scripts.Delivery;
using Xenocode.Features.UserSupplies.Scripts.Domain.Model;
using Xenocode.Features.UserSupplies.Scripts.Domain.Presentation;
using Xenocode.Features.UserSupplies.Scripts.Domain.Services;

namespace Xenocode.Features.UserSupplies.Scripts.Domain.Provider
{
    public static class UserSuppliesProvider
    {
        private static IUserSuppliesService _userSuppliesService;
        
        public static void Present(IUserSuppliesView view, UserSuppliesNet user) 
        {
            _userSuppliesService = GetUserSuppliesService();
            _userSuppliesService.Initialize(user); 
            
            new UserSuppliesPresenter(view, user, _userSuppliesService);
        }
        
        public static IUserSuppliesService GetUserSuppliesService() 
        {
            return _userSuppliesService ??= new UserSuppliesService();
        }
    }
}