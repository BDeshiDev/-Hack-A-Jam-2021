﻿using System;
using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Anything that can take damage.
    /// </summary>
    public class CombatEntity: MonoBehaviour, IDamagable
    {
        [SerializeField]protected HealthComponent healthComponent;
        protected virtual void Awake()
        {
            if (healthComponent == null)
                healthComponent = GetComponent<HealthComponent>();
        }
        /// <summary>
        /// Calc defence here if needed
        /// </summary>
        /// <param name="damage"></param>
        public virtual void processDamage(ref DamageInfo damage)
        {
            //do nothing
        }
        /// <summary>
        /// Just changes healthcomponent
        /// </summary>
        /// <param name="damage"></param>
        public virtual void takeDamage(DamageInfo damage)
        {
            processDamage(ref damage);
            healthComponent.modifyAmount(damage.healthDamage);
        }
    }
}