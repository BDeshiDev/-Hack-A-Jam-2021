using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat.Enemies
{
    public class Gun: MonoBehaviour
    {
        public Transform shotPoint;
        //use for muzzle flashes
        public UnityEvent ShotFired;
        // not needed for the jam
        [SerializeField] private UnityEvent BlankFired;

        [CanBeNull][SerializeField] private AmmoComponent ammoComponent;

        public virtual void shoot(GunShot gunShot, TargetResolverComponent targetResolverComponent)
        {
            //if we have a ammocomponent and we don't have enough ammo, don't shoot
            //also 1 round = 1 ammo
            if (ammoComponent != null && !ammoComponent.tryUse(1))
            {
                BlankFired.Invoke();
                return;
            }

            gunShot.shoot(shotPoint, targetResolverComponent);
            ShotFired.Invoke();
        }
    }
}