using System;
using UnityEngine;

namespace Core.Combat.ShotPattern
{
    [Serializable]
    public abstract class ShotPattern
    {
        public abstract void shoot(Projectile prefab, Transform shotPoint, TargettingInfo targettingInfo);

        protected Projectile spawn(Projectile prefab)
        {
            return GameplayPoolManager.Instance.projectilePool.get(prefab); 
        }
    }
}