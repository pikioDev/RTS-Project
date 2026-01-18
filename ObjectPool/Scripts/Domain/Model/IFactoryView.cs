using System;
using UnityEngine;
using Xenocode.Features.Unit.Scripts.Domain.Model;

namespace Xenocode.Features.ObjectPool.Scripts.Domain.Model
{
    public interface IFactoryView
    {
        int InitialPoolSize { get; }
        
        Func<Transform, ulong, IUnitView> OnGetObjectFromPool { get; set; }
        
        Action<IUnitView> OnReturnObjectToPool { get; set; }
        IUnitView CreateUnit();
    }
}