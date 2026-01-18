using System;
using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Model;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Services
{
    public class UnitPoolService<T> : IUnitPoolService<T>
    {
        private readonly Func<T> _factoryMethod;
        private readonly Action<T, Transform, ulong, UnitType, int> _turnOnCallback;
        private readonly Action<T> _turnOffCallback;
        private List<T> _currentStock;
        
        public UnitPoolService(Func<T> factoryMethod, Action<T, Transform, ulong, UnitType, int> turnOnCallback, Action<T> turnOffCallback, int initialAmount)
        {
            _factoryMethod = factoryMethod;
            _turnOnCallback = turnOnCallback;
            _turnOffCallback = turnOffCallback;
            FillPool(initialAmount);
        }

        private void FillPool(int initialAmount)
        {
            _currentStock = new List<T>();
            for (int i = 0; i < initialAmount; i++)
            {
                T obj = _factoryMethod();
                _turnOffCallback(obj);
                _currentStock.Add(obj);
            }
        }

        public T GetObject(Transform spawnPoint, ulong ownerClientId, UnitType unitType, int teamIndex)
        {
            T result;
            if (_currentStock.Count == 0) {
                result = _factoryMethod();
            } else {
                result = _currentStock[0];
                _currentStock.RemoveAt(0);
            }

        
            _turnOnCallback(result, spawnPoint, ownerClientId, unitType, teamIndex);
    
            return result;
        }
        
        public void ReturnObjectToPool(T obj)
        {
            _currentStock.Add(obj);
            _turnOffCallback(obj);
        }
    }
}
