using Core.Combat.ShotPattern;
using UnityEngine;

namespace Core.Combat
{
    public class Gun: MonoBehaviour
    {
        public Transform shotPoint;
        //#TODO pools
        public Projectile prefab;
        public EnemyTargetResolver targetter;

        [SerializeReferenceButton]
        [SerializeReference] 
        [SerializeField]
        private ShotPattern.ShotPattern shotPattern = new FanShotPattern();
        public void shoot()
        {
            shotPattern.shoot(prefab, shotPoint, targetter.TargettingInfo);

            // Instantiate(prefab).initialize(shotPoint.position,shotPoint.right,targetter.TargettingInfo);
        }
    }
}