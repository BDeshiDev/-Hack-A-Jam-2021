using System;
using Core.Combat.Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public abstract class EnemyEntity: BlobEntity
    {
        [SerializeField] protected HypnoComponent hypnoComponent;
        public EnemyTargetResolver Targetter => targetter;
        public bool IsHypnotized => hypnoComponent.IsHypnotized;

        [SerializeField] protected EnemyTargetResolver targetter;

        /// <summary>
        /// at t = T% hypnodamage taken at T%/100 health and so on
        /// </summary>
        public AnimationCurve hypnoDamageVsHealthCurve = AnimationCurve.EaseInOut(0,1,1,0);
        public float hypnoDamageHealthScalingFactor = 5;
        public float hypnoRecoveryPerHealthDamage = .5f;


        /// <summary>
        /// Health and hypnosis are modified
        /// </summary>
        /// <param name="damage"></param>
        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            float hypnoDamage = calcHypnoDamage(damage);
            Debug.Log(damage + "actual hypno->" + hypnoDamage
                      + "factor " + healthComponent.Ratio + "  " + 
                      hypnoDamageVsHealthCurve.Evaluate(healthComponent.Ratio ));
            hypnoComponent.modifyAmount(hypnoDamage);
        }

        public float calcHypnoDamage(DamageInfo damage)
        {
            return ( 1 +  hypnoDamageVsHealthCurve.Evaluate(healthComponent.Ratio) * hypnoDamageHealthScalingFactor)
                   * damage.hypnoDamage
                   - (hypnoComponent.IsHypnotized? 1 : 0) * hypnoRecoveryPerHealthDamage;
        }
        
        protected override void Awake()
        {
            base.Awake();
            hypnoComponent = GetComponent<HypnoComponent>();
            spriter = GetComponent<SpriteRenderer>();
            targetter = GetComponent<EnemyTargetResolver>();
        }
        
        



        private void OnEnable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Hypnotized += OnHypnotized;
                hypnoComponent.HypnosisRecovered += HypnosisRecovered;
            }
            EnemyTracker.addNewActiveEnemy(this);
        }
        
        private void OnDisable()
        {
            if(hypnoComponent != null)
            {
                hypnoComponent.Hypnotized -= OnHypnotized;
                hypnoComponent.HypnosisRecovered -= HypnosisRecovered;
            }
            EnemyTracker.removeInactiveEnemy(this);

        }

        protected void HypnosisRecovered()
        {
            targetter.handleNormalState();
            targetter.gameObject.layer = targetter.TargettingInfo.EnemyLayer.LayerIndex;
            spriter.color = Color.yellow;
        }

        protected void OnHypnotized()
        {
            Debug.Log("hypnotised " +gameObject , gameObject);

            targetter.handleHypnosis();
            targetter.gameObject.layer = targetter.TargettingInfo.PlayerLayer.LayerIndex;
            
            spriter.color = Color.cyan;
        }
    }
    
    
}