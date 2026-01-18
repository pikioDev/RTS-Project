using System.Collections.Generic;
using Xenocode.Features.Matchmaking.Scripts.Domain.Model;
using Xenocode.Features.UserSupplies.Scripts.Delivery;

namespace Xenocode.Features.Match.Scripts.Domain.Infrastructure
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly Dictionary<ulong, UserSuppliesNet> _players = new();

        public void Add(ulong clientId, UserSuppliesNet entity)
        {
            if (!entity) return;
            _players[clientId] = entity;
        }

        public void Remove(ulong clientId)
        {
            _players.Remove(clientId);
        }

        public UserSuppliesNet GetPlayer(ulong clientId) => 
            _players.TryGetValue(clientId, out var entity) ? entity : null;

        public IEnumerable<UserSuppliesNet> GetAllSuppliesEntities() => _players.Values;

        public void Clear()
        {
            _players.Clear();
        }
    }
}