using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Combat.ShotPattern
{
    [Serializable]
    public class FanShotPattern: ShotPattern
    {
        public float spawnRadius = .25f;
        [Range(0,360)]
        public float angle = 60;
        public int count = 5;
        public override void shoot(Projectile prefab, Transform shotPoint, TargettingInfo targettingInfo)
        {
            if(count < 0)
                return;
        
            float shotAngleGap;
            float startAngle;
            if (count % 2 == 0)
            {
                shotAngleGap = angle / (count-1);
                startAngle = -(angle / 2);
            }
            else
            {
                shotAngleGap = angle / (count-1);
                startAngle = -shotAngleGap *.5f * (count-1);
            }


            Vector3 shotDir = shotPoint.right;
            for (int i = 0; i < count; i++)
            {
                Vector3 curShotDir =  Quaternion.AngleAxis(startAngle + i * shotAngleGap, Vector3.forward) 
                                      * shotDir;

                // var p = Object.Instantiate(prefab);
                var p = spawn(prefab);

                p.initialize(shotPoint.position + curShotDir * spawnRadius, curShotDir, targettingInfo);
            }

        }
    }
}