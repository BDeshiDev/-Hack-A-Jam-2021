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
        public bool IsHypnotized => CurState == HypnosisState.Hypnotized;
        public bool IsBerserked => CurState == HypnosisState.Berserk;
        
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
        
        public HypnosisState CurState { get; private set; }

        private HealthComponent healthComponent;
        

        public override void handleCapped()
        {
            if (!IsBerserked && !IsHypnotized)
            {
                hypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate;
                CurState = HypnosisState.Hypnotized;
                Hypnotized?.Invoke();
            }

            base.handleCapped();
        }
        

        public override void handleEmptied()
        {
            if (!IsBerserked && healthComponent.Ratio < berserkThreshold)
            {
                CurState = HypnosisState.Berserk;
                Berserked?.Invoke();
            }else if (IsHypnotized)
            {
                hypnosisRecoveryRate = normalHypnosisRecoveryRate;
                
                HypnosisRecovered?.Invoke();
            }
            base.handleEmptied();
        }
        
        public float calcHypnoDamage(DamageInfo damage)
        {
            return ( 1 +  hypnoDamageVsHealthCurve.Evaluate(healthComponent.Ratio) * hypnoDamageHealthScalingFactor)
                   * damage.hypnoDamage
                   - (IsHypnotized? 1 : 0) * hypnoRecoveryPerHealthDamage;
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
                hypnoDOTIncreaseTimer.updateTimer(Time.deltaTime);
                
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

        

    }
    
    public enum HypnosisState
    {
        Normal,
        Hypnotized,
        Berserk,
    }
}