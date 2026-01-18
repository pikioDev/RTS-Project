using System.Collections;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Provider;

namespace Xenocode.Features.Strike.Scripts.Delivery
{
    public class MeleeStrike : Domain.Model.Strike
    {
        public override void Execute(Transform pos, Vector3 target, float travelTime)
        {
            SetPositionAndRotations(pos);
            SetParent(pos);
            StartCoroutine(Despawn(travelTime));
        }

        IEnumerator Despawn(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime + 0.8f);
            transform.parent = null;
            StrikeFactoryService.ReturnObjectToPool(this);
        }
    }
}
