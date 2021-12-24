using System;
using BDeshi.Utility;
using Core.Combat.Enemies;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public abstract class EnemyEntity: BlobEntity
    {
        [SerializeField] protected HypnoComponent hypnoComponent;
        public EnemyTargetResolver Targetter => targetter;
        public bool IsHypnotized => hypnoComponent.IsHypnotized;

        [SerializeField] protected EnemyTargetResolver targetter;
        public float hypnoDOTstartRate = .25f;
        public float hypnoDOTMaxRate = 2f;
        public FiniteTimer hypnoDOTIncreaseTimer = new FiniteTimer(0,9f);
        public bool hypnoDOTActive = false;
        
        
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

        protected virtual void Update()
        {
            if (hypnoComponent.IsHypnotized)
            {
                hypnoDOTIncreaseTimer.updateTimer(Time.deltaTime);
            }

            if (hypnoDOTActive)
            {
                var dotDamage = Mathf.Lerp(hypnoDOTstartRate, hypnoDOTMaxRate, hypnoDOTIncreaseTimer.Ratio) * Time.deltaTime;
                healthComponent.reduceAmount(dotDamage);
            }
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
            targetter.gameObject.layer = targetter.TargettingInfo.NormalLayer.LayerIndex;
        }

        protected void OnHypnotized()
        {
            Debug.Log("hypnotised " +gameObject , gameObject);

            hypnoDOTActive = true;

            targetter.handleHypnosis();
            targetter.gameObject.layer = targetter.TargettingInfo.HypnotizedLayer.LayerIndex;
        }
    }
    
    
}