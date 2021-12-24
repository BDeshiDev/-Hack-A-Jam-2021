using System;
using BDeshi.Utility;
using Core.Combat.Enemies;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public class HypnoComponent: ResourceComponent
    {
        public float hypnosisRecoveryRate = 1;
        public float normalHypnosisRecoveryRate = 1;
        public float hypnotizedStateHypoRecoveryRate = 3;
        public bool IsHypnotized => curState == HypnosisState.Hypnotized;
        public bool IsBerserked => curState == HypnosisState.Berserk;
        
        public float hypnoDOTstartRate = .25f;
        public float hypnoDOTMaxRate = 2f;
        public FiniteTimer hypnoDOTIncreaseTimer = new FiniteTimer(0,9f);
        public bool hypnoDOTActive = false;
        
        private float berserkThreshold = .15f;
        
        /// <summary>
        /// at t = T% hypnodamage taken at T%/100 health and so on
        /// </summary>
        public AnimationCurve hypnoDamageVsHealthCurve = AnimationCurve.EaseInOut(0,1,1,0);
        public float hypnoDamageHealthScalingFactor = 5;
        public float hypnoRecoveryPerHealthDamage = .5f;
        
        public event Action HypnosisRecovered;
        public event Action Hypnotized;
        public event Action Berserked;

        private bool forceBerserk = false;

        public HypnosisState CurState => curState;
        [SerializeField] private HypnosisState curState;

        private HealthComponent healthComponent;
        

        public override void handleCapped()
        {
            if (!IsBerserked && !IsHypnotized)
            {
                hypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate;
                curState = HypnosisState.Hypnotized;
                hypnoDOTActive = true;
                Hypnotized?.Invoke();
            }

            base.handleCapped();
        }

        public void forceBerserkState()
        {
            forceBerserk = true;
            forceEmpty();
        }


        public override void handleEmptied()
        {
            if (!IsBerserked && (healthComponent.Ratio < berserkThreshold || forceBerserk))
            {
                curState = HypnosisState.Berserk;
                Berserked?.Invoke();
            }else if (IsHypnotized)
            {
                hypnosisRecoveryRate = normalHypnosisRecoveryRate;
                curState = HypnosisState.Normal;

                HypnosisRecovered?.Invoke();
            }
            base.handleEmptied();
        }
        
        public float calcHypnoDamage(DamageInfo damage)
        {
            return ( 1 +  hypnoDamageVsHealthCurve.Evaluate(healthComponent.Ratio) * hypnoDamageHealthScalingFactor)
                   * damage.hypnoDamage
                   - (IsHypnotized? 1 : 0) *damage.healthDamage * hypnoRecoveryPerHealthDamage;
        }
        
        
        private void Awake()
        {
            hypnosisRecoveryRate = normalHypnosisRecoveryRate;
            healthComponent = GetComponent<HealthComponent>();
        }
        protected virtual void Update()
        {
            if (IsHypnotized)
            {
                hypnoDOTIncreaseTimer.safeUpdateTimer(Time.deltaTime);
                
                var dotDamage = Mathf.Lerp(hypnoDOTstartRate, hypnoDOTMaxRate, hypnoDOTIncreaseTimer.Ratio) * Time.deltaTime;
                healthComponent.reduceAmount(dotDamage);
            }
            
            reduceAmount(Time.deltaTime * hypnosisRecoveryRate);

            
            //might be too op 
            // if (hypnoDOTActive)
            // {
            //     var dotDamage = Mathf.Lerp(hypnoDOTstartRate, hypnoDOTMaxRate, hypnoDOTIncreaseTimer.Ratio) * Time.deltaTime;
            //     healthComponent.reduceAmount(dotDamage);
            // }
        }


        public override void modifyAmount(float changeAmount)
        {
            if(IsBerserked)
                return;
            base.modifyAmount(changeAmount);
        }
    }
    
    public enum HypnosisState
    {
        Normal,
        Hypnotized,
        Berserk,
    }
}