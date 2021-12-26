using System;
using BDeshi.BTSM;
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
        public bool wasHypnotizedBefore = false;
        
        private float berserkThreshold = .15f;
        
        /// <summary>
        /// at t = T% hypnodamage taken at T%/100 health and so on
        /// </summary>
        public AnimationCurve hypnoDamageVsHealthCurve = AnimationCurve.EaseInOut(0,1,1,0);
        public float hypnoDamageHealthScalingFactor = 1;
        public float hypnoRecoveryPerHealthDamage = .5f;

        public event Action<HypnoComponent> HypnosisRecovered;
        public event Action<HypnoComponent> Hypnotized;
        public event Action<HypnoComponent> Berserked;

        private bool forceBerserk = false;

        public HypnosisState CurState => curState;
        [SerializeField] private HypnosisState curState;

        private HealthComponent healthComponent;
        private float dotRate;
        [SerializeField] float normalStateDOTMultiplier = .3f;


        public override void handleCapped()
        {
            if (!IsBerserked && !IsHypnotized)
            {
                curHypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate;
                enterState(HypnosisState.Hypnotized);
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
                forceBerserk = false;
                enterState(HypnosisState.Berserk);
            }else if (IsHypnotized)
            {
                curHypnosisRecoveryRate = normalHypnosisRecoveryRate;
                enterState(HypnosisState.Normal);
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
            healthComponent = GetComponent<HealthComponent>();
            initialize();
        }

        public void initialize()
        {
            cur = 0;
            curHypnosisRecoveryRate = normalHypnosisRecoveryRate;
            hypnoRecoverySpeedChangeTimer.reset();
            hypnoDOTIncreaseTimer.reset();
            wasHypnotizedBefore = false;
            enterState(HypnosisState.Normal);
        }

        protected virtual void Update()
        {
            if (IsHypnotized)
            {
                hypnoDOTIncreaseTimer.safeUpdateTimer(Time.deltaTime);
                
                dotRate = Mathf.Lerp(hypnoDOTstartRate, hypnoDOTMaxRate, hypnoDOTIncreaseTimer.Ratio) ;
                healthComponent.reduceAmount(dotRate * Time.deltaTime);
                
                hypnoRecoverySpeedChangeTimer.safeUpdateTimer(Time.deltaTime);
                curHypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate = Mathf.Lerp(
                    startingHypnosisRecoveryRate,
                    finalHypnosisRecoveryRate,
                    hypnoRecoverySpeedCurve.Evaluate(hypnoRecoverySpeedChangeTimer.Ratio)
                );
            }
            
            reduceAmount(Time.deltaTime * curHypnosisRecoveryRate);

            
             if (wasHypnotizedBefore)
             {
                 healthComponent.reduceAmount(dotRate * normalStateDOTMultiplier * Time.deltaTime);
             }
            
            
        }


        public override void modifyAmount(float changeAmount)
        {
            if(IsBerserked)
                return;
            base.modifyAmount(changeAmount);
        }

        public void enterState(HypnosisState state)
        {
            curState = state;
            switch (curState)
            {
                case HypnosisState.Normal:
                    HypnosisRecovered?.Invoke(this);
                    break;
                case HypnosisState.Berserk:
                    curState = HypnosisState.Berserk;   
                    Berserked?.Invoke(this);
                    break;
                case HypnosisState.Hypnotized:
                    curState = HypnosisState.Hypnotized;
                    wasHypnotizedBefore = true;
                    Hypnotized?.Invoke(this);
                    break;
                default:
                    break;
            }
        }
    }
    
    public enum HypnosisState
    {
        Normal,
        Hypnotized,
        Berserk,
    }
}