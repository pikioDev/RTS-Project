using System;
using System.Collections.Generic;
using UnityEngine;
using Xenocode.Features.ObjectPool.Scripts.Domain.Model;
using Xenocode.Features.Strike.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Services
{
    public class StrikePoolService<IStrike> : IStrikePoolService<IStrike>
    {
        private readonly Func<IStrike> _factoryMethod;
        private readonly Action<IStrike, Transform, StrikeType> _turnOnCallback;
        private readonly Action<IStrike> _turnOffCallback;
        private List<IStrike> _currentStock;
        
        public StrikePoolService(Func<IStrike> factoryMethod, Action<IStrike, Transform, StrikeType> turnOnCallback,
            Action<IStrike> turnOffCallback, int initialAmount)
        {
            _factoryMethod = factoryMethod;
            _turnOnCallback = turnOnCallback;
            _turnOffCallback = turnOffCallback;
            FillPool(initialAmount);
        }

        private void FillPool(int initialAmount)
        {
            _currentStock = new List<IStrike>();
            for (int i = 0; i < initialAmount; i++)
            {
                IStrike obj = _factoryMethod();
                _turnOffCallback(obj);
                _currentStock.Add(obj);
            }
        }

        public IStrike GetObject(Transform spawnPoint, StrikeType strikeType)
        {
            IStrike result;
            if (_currentStock.Count == 0) {
                result = _factoryMethod();
            } else {
                result = _currentStock[0];
                _currentStock.RemoveAt(0);
            }
            _turnOnCallback(result, spawnPoint, strikeType);
    
            return result;
        }

        public void ReturnObject(IStrike obj)
        {
            _currentStock.Add(obj);
            _turnOffCallback(obj);
        }
    }
}

