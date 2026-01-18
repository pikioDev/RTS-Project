using System;
using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Model;
using Xenocode.Features.ObjectPool.Scripts.Domain.Services;
using Xenocode.Features.Strike.Scripts.Domain.Model;


namespace Xenocode.Features.ObjectPool.Scripts.Domain.Provider
{
    public static class StrikeFactoryService
    {
        private static Dictionary<StrikeType, IStrikePoolService<IStrike>> _poolLookups;
        private static bool _isInitialized;
        
        static StrikeFactoryService()
        {
            Initialize(); // Llamamos a un método separado para una inicialización más limpia.
        }
        
        static void Initialize()
        {
            if (_isInitialized) return;

            _poolLookups = new Dictionary<StrikeType, IStrikePoolService<IStrike>>();
            
            var config = Resources.Load<StrikeFactoryConfig>("StrikeFactoryConfig");
            
            
            foreach (var entry in config.GetAllEntries())
            {
                var strikePrefab = entry.GetPrefab(); 
                var initialSize = entry.GetInitialSize();
                var strikeType = entry.GetStrikeType();
                
                Func<IStrike> factoryMethod = () => (IStrike)UnityEngine.Object.Instantiate((Strike.Scripts.Domain.Model.Strike)strikePrefab);
                
                Action<IStrike, Transform, StrikeType> turnOnCallback = (bullet, transform, type) =>
                {
                    bullet.SetStrikeType(type);
                    bullet.SetPositionAndRotations(transform);
                    ((MonoBehaviour)bullet).gameObject.SetActive(true);
                };
                
                Action<IStrike> turnOffCallback = (strike) =>
                {
                    ((MonoBehaviour)strike).gameObject.SetActive(false);
                };
                
                var poolService = new StrikePoolService<IStrike>(
                    factoryMethod, 
                    (strike, transform, type) => turnOnCallback(strike, transform, type),
                    turnOffCallback, 
                    initialSize
                );

                _poolLookups.Add(strikeType, poolService);
            }
            
            _isInitialized = true;
        }
        
        public static IStrike GetObj(StrikeType type, Transform spawnPosition)
        {
            if (!_isInitialized) return null;

            if (_poolLookups.TryGetValue(type, out var poolService))
            {
                return poolService.GetObject(spawnPosition, type); 
            }
            
            return null;
        }
        
        public static void ReturnObjectToPool(IStrike strike)
        {
            if (!_isInitialized) return;
            var type = strike.GetStrikeType(); 
            if (_poolLookups.TryGetValue(type, out var poolService))
            {
                poolService.ReturnObject(strike);
            }
        }
        
    }
    
}