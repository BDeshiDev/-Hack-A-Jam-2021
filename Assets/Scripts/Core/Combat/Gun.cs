using UnityEngine;

namespace Core.Combat
{
    public class Gun: MonoBehaviour
    {
        public Transform shotPoint;
        //#TODO pools
        public Projectile prefab;
        public TargetResolverComponent targetter;
        public void shoot()
        {
            Instantiate(prefab).initialize(shotPoint.position,shotPoint.right,targetter.targettingInfo);
        }
    }
}