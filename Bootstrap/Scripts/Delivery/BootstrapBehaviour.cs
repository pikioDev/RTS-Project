using UnityEngine;
using Xenocode.Features.Audio.Scripts.Domain.Provider;
using Xenocode.Features.Login.Scripts.Providers;

namespace Xenocode.Features.Bootstrap.Scripts.Delivery
{
    public class BootstrapBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            LoginProvider.GetLoginService().LoginWithUnity();
            AudioProvider.InitializeService();
        }
    }
}
