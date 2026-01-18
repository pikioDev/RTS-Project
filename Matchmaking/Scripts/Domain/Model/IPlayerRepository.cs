using System.Collections.Generic;
using Xenocode.Features.UserSupplies.Scripts.Delivery;

namespace Xenocode.Features.Matchmaking.Scripts.Domain.Model
{
    public interface IPlayerRepository
    {
        void Add(ulong clientId, UserSuppliesNet entity);
        IEnumerable<UserSuppliesNet> GetAllSuppliesEntities();
        UserSuppliesNet GetPlayer(ulong clientId); 
        void Remove(ulong clientId);
        void Clear();
    }
}