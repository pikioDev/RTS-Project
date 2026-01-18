using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Provider;

namespace Xenocode.Features.Strike.Scripts.Delivery
{
    public class ParableStrike : Domain.Model.Strike
    {
        [SerializeField] private float height = 1f;
        
        public override void Execute(Transform pos,Vector3 target, float travelTime)
        {
            SetPositionAndRotations(pos);
            _ = AnimateParabola(target, travelTime, height);
            StartCoroutine(Despawn(travelTime));
        }

        IEnumerator Despawn(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            StrikeFactoryService.ReturnObjectToPool(this);
        }

        private async UniTask AnimateParabola(Vector3 target, float travelTime, float height)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0;

            while (elapsedTime < travelTime)
            {
                float t = elapsedTime / travelTime;
                Vector3 currentPosition = CalculateParabolicPosition(startPosition, target, t, height);
                transform.position = currentPosition;
                
                float nextT = (elapsedTime + Time.deltaTime) / travelTime;
                Vector3 nextPosition = CalculateParabolicPosition(startPosition, target, nextT, height);
                
                if (Vector3.Distance(nextPosition, currentPosition) > 0.001f)
                {
                    transform.forward = (nextPosition - currentPosition).normalized;
                }

                elapsedTime += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
            
            transform.position = target;
        }

        private Vector3 CalculateParabolicPosition(Vector3 start, Vector3 end, float t, float height)
        {
            Vector3 linearPosition = Vector3.Lerp(start, end, t);
            float yOffset = height * 4 * t * (1 - t);
            return new Vector3(linearPosition.x, linearPosition.y + yOffset, linearPosition.z);
        }
        
    }
}