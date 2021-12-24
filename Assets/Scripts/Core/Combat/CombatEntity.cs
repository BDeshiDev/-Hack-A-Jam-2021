using System;
using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Anything that can take damage.
    /// </summary>
    public class CombatEntity: MonoBehaviour, IDamagable
    {
        public HealthComponent HealthComponent=> healthComponent;
        [SerializeField] protected HealthComponent healthComponent;
        public bool isInvulnerable;
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
            if(isInvulnerable)
                return;
            
            processDamage(ref damage);
            healthComponent.reduceAmount(damage.healthDamage);
        }
    }
}