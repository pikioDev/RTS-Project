using System;
using System.Collections;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Provider;

namespace Xenocode.Features.Strike.Scripts.Delivery
{
    public class DirectionalStrike : Strike.Scripts.Domain.Model.Strike
    {
        private Vector3 _velocity;
      
        public override void Execute(Transform spawnPoint, Vector3 target, float travelTime)
        {
            SetPositionAndRotations(spawnPoint);
            transform.LookAt(target);
            
            _velocity = transform.forward * CalculateSpeed(spawnPoint, target, travelTime);

            StartCoroutine(Despawn(travelTime));
        }

        private static float CalculateSpeed(Transform spawnPoint, Vector3 target, float travelTime)
        {
            return Vector3.Distance(spawnPoint.position, target) / Math.Max(travelTime, 0.1f);
        }

        IEnumerator Despawn(float lifeTime)
        {
            var elapsedTime = 0f;
            while (elapsedTime < lifeTime)
            {
                transform.position += _velocity * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            StrikeFactoryService.ReturnObjectToPool(this);
        }
    }
}