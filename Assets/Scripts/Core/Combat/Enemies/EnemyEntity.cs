using System;
using UnityEngine;

namespace Core.Combat
{
    public abstract class EnemyEntity: BlobEntity
    {
        [SerializeField] protected HypnoComponent hypnoComponent;
        [SerializeField] protected bool isHypnotized = false;

        /// <summary>
        /// at t = T% hypnodamage taken at T%/100 health and so on
        /// </summary>
        public AnimationCurve hypnoDamageVsHealthCurve = AnimationCurve.EaseInOut(0,1,1,0);
        public float hypnoDamageHealthScalingFactor = 5;
        public float hypnoRecoveryPerHealthDamage = .5f;

        public Transform target;

        /// <summary>
        /// Health and hypnosis are modified
        /// </summary>
        /// <param name="damage"></param>
        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            float hypnoDamage = calcHypnoDamage(damage);
            hypnoComponent.modifyAmount(hypnoDamage);
        }

        public float calcHypnoDamage(DamageInfo damage)
        {
            return ( 1 + damage.hypnoDamage * hypnoDamageVsHealthCurve.Evaluate(healthComponent.Ratio))
                   * hypnoDamageHealthScalingFactor
                   - (isHypnotized? 1 : 0) * hypnoRecoveryPerHealthDamage;
        }
        
        protected override void Awake()
        {
            base.Awake();
            hypnoComponent = GetComponent<HypnoComponent>();
            spriter = GetComponent<SpriteRenderer>();

            target = GameObject.FindWithTag("Player").transform;
        }



        private void OnEnable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Capped += OnHypnotized;
                hypnoComponent.Emptied += HypnosisRecovered;
            }
        }
        
        private void OnDisable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Capped -= OnHypnotized;
                hypnoComponent.Emptied -= HypnosisRecovered;
            }
        }

        protected void HypnosisRecovered(ResourceComponent obj)
        {
            if (isHypnotized)
            {
                Debug.Log("revcover hypnotise " +gameObject , gameObject);
            }
            spriter.color = Color.yellow;
            isHypnotized = false;
        }

        protected void OnHypnotized(ResourceComponent obj)
        {
            isHypnotized = true;
            Debug.Log("hypnotised " +gameObject , gameObject);
            
            spriter.color = Color.cyan;

        }
    }
    
    
}