using Core.Combat.CombatResources;
using Core.Combat.Damage;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat.Entities
{
    /// <summary>
    /// Anything that can take damage.
    /// </summary>
    public class CombatEntity: MonoBehaviour, IDamagable
    {
        public HealthComponent HealthComponent=> healthComponent;
        [SerializeField] protected HealthComponent healthComponent;
        public bool isInvulnerable;
        public bool CanDie = true;
        public UnityEvent ActualDeathEvent;
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

        protected void invokeDeathEvent()
        {
            ActualDeathEvent.Invoke();
        }

    }
}