using Cysharp.Threading.Tasks;

namespace Xenocode.Features.Matchmaking.Scripts.Domain.Services
{
    public interface IMatchmakingService
    {
        UniTask CreateOrJoin();
    }
}