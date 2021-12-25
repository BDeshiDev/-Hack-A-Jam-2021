using Core.Combat.ShotPattern;
using UnityEngine;

namespace Core.Combat
{
    public class GunShot: MonoBehaviour
    {
        //#TODO pools
        public Projectile prefab;

        [SerializeReferenceButton]
        [SerializeReference] 
        [SerializeField]
        private ShotPattern.ShotPattern shotPattern = new FanShotPattern();
        public void shoot(Transform shotPoint, TargetResolverComponent targetter)
        {
            shotPattern.shoot(prefab, shotPoint, targetter.TargettingInfo);

            // Instantiate(prefab).initialize(shotPoint.position,shotPoint.right,targetter.TargettingInfo);
        }
    }
}