﻿using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat.Enemies
{
    public class Gun: MonoBehaviour
    {
        public Transform shotPoint;

        [CanBeNull][SerializeField] private AmmoComponent ammoComponent;
        public AmmoComponent AmmoComponent => ammoComponent;
        public bool CanFire => ammoComponent == null || ammoComponent.CurAmmo >= 1;

        public UnityEvent  ShotFired; 
        public virtual void shoot(GunShot gunShot, TargetResolverComponent targetResolverComponent)
        {
            //if we have a ammocomponent and we don't have enough ammo, don't shoot
            //also 1 round = 1 ammo
            if (ammoComponent != null && !ammoComponent.tryUse(1))
            {
                return;
            }

            gunShot.shoot(shotPoint, targetResolverComponent);

            
            ShotFired.Invoke();
        }
    }
}