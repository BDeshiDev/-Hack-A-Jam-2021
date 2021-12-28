using System;
using Core.Combat.Shooting;
using Core.Combat.Targetting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Combat.ShotPattern
{
    [Serializable]
    public class SingleShotPattern: ShotPattern
    {
        public override void shoot(Projectile prefab,Transform shotPoint, TargettingInfo targettingInfo)
        {
            // var p = Object.Instantiate(prefab);
            var p = spawn(prefab);

            p.initialize(shotPoint.position, shotPoint.right, targettingInfo );
        }
    }
}