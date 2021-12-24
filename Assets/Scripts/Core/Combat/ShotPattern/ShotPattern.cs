using System;
using DefaultNamespace;
using UnityEngine;

namespace Core.Combat.ShotPattern
{
    [Serializable]
    public abstract class ShotPattern
    {
        public abstract void shoot(Projectile prefab, Transform shotPoint, TargettingInfo targettingInfo);

        protected Projectile spawn(Projectile prefab)
        {
            return PoolManager.Instance.projectilePool.get(prefab); 
        }
    }
}