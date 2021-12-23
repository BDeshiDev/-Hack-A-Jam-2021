using UnityEngine;

namespace Core.Combat
{
    public class Gun: MonoBehaviour
    {
        public Transform shotPoint;
        //#TODO pools
        public Projectile prefab;
        
        public void shoot()
        {
            Instantiate(prefab).initialize(shotPoint.position,shotPoint.right);
        }
    }
}