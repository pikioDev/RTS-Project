using System;

namespace Xenocode.Features.SuppliesManager.Scripts.Domain.Model
{
    public interface ISuppliesManager
    {
        event Action<float> OnServerTick;
    }
}