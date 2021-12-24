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
        [FormerlySerializedAs("hypnosisRecoveryRate")] 
        public float startingHypnosisRecoveryRate = 1;
        public float finalHypnosisRecoveryRate = 10;
        public AnimationCurve hypnoRecoverySpeedCurve = AnimationCurve.EaseInOut(0,0,1,1);
        public FiniteTimer hypnoRecoverySpeedChangeTimer = new FiniteTimer(0, 15f);
        [SerializeField] float curHypnosisRecoveryRate = 1;
        
        public float normalHypnosisRecoveryRate = 1;
        public float hypnotizedStateHypoRecoveryRate = 3;
        public bool IsHypnotized => curState == HypnosisState.Hypnotized;
        public bool IsBerserked => curState == HypnosisState.Berserk;
        public bool IsInBerserkRange => healthComponent.Ratio < berserkThreshold;
        
        public float hypnoDOTstartRate = .25f;
        public float hypnoDOTMaxRate = 2f;
        public FiniteTimer hypnoDOTIncreaseTimer = new FiniteTimer(0,9f);
        public bool hypnoDOTActive = false;
        
        private float berserkThreshold = .15f;
        
        /// <summary>
        /// at t = T% hypnodamage taken at T%/100 health and so on
        /// </summary>
        public AnimationCurve hypnoDamageVsHealthCurve = AnimationCurve.EaseInOut(0,1,1,0);
        public float hypnoDamageHealthScalingFactor = 1;
        public float hypnoRecoveryPerHealthDamage = .5f;
        /// <summary>
        /// gradually lose maxHypno according to this.
        /// </summary>
        public float maxHypnoLossRate;

        public float actualMaxHypno;
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
                curHypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate;
                curState = HypnosisState.Hypnotized;
                hypnoDOTActive = true;
                Hypnotized?.Invoke();
            }

            base.handleCapped();
        }
        
        [ContextMenu("force berserk")]
        public void forceBerserkState()
        {
            forceBerserk = true;
            forceEmpty();
        }


        public override void handleEmptied()
        {
            if (!IsBerserked && (IsInBerserkRange || forceBerserk))
            {
                curState = HypnosisState.Berserk;
                Berserked?.Invoke();
            }else if (IsHypnotized)
            {
                curHypnosisRecoveryRate = normalHypnosisRecoveryRate;
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
            curHypnosisRecoveryRate = normalHypnosisRecoveryRate;
            healthComponent = GetComponent<HealthComponent>();
        }
        protected virtual void Update()
        {
            if (IsHypnotized)
            {
                hypnoDOTIncreaseTimer.safeUpdateTimer(Time.deltaTime);
                
                var dotDamage = Mathf.Lerp(hypnoDOTstartRate, hypnoDOTMaxRate, hypnoDOTIncreaseTimer.Ratio) * Time.deltaTime;
                healthComponent.reduceAmount(dotDamage);
                
                hypnoRecoverySpeedChangeTimer.safeUpdateTimer(Time.deltaTime);
                curHypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate = Mathf.Lerp(
                    startingHypnosisRecoveryRate,
                    finalHypnosisRecoveryRate,
                    hypnoRecoverySpeedCurve.Evaluate(hypnoRecoverySpeedChangeTimer.Ratio)
                );
            }
            
            reduceAmount(Time.deltaTime * curHypnosisRecoveryRate);

            
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