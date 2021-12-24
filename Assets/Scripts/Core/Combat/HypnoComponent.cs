using System;
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
        public bool IsHypnotized => isHypnotized;
        [SerializeField] bool isHypnotized  = false;
        
        public event Action HypnosisRecovered;
        public event Action Hypnotized;

        private void Awake()
        {
            hypnosisRecoveryRate = normalHypnosisRecoveryRate;
        }

        private void Update()
        {
            reduceAmount(Time.deltaTime * hypnosisRecoveryRate);
        }

        public override void handleCapped()
        {
            if (!IsHypnotized)
            {
                isHypnotized = true;
                hypnosisRecoveryRate = hypnotizedStateHypoRecoveryRate;
                
                Hypnotized?.Invoke();
            }

            base.handleCapped();
        }

        public override void handleEmptied()
        {
            if (IsHypnotized)
            {
                isHypnotized = false;
                hypnosisRecoveryRate = normalHypnosisRecoveryRate;
                
                HypnosisRecovered?.Invoke();
            }
            base.handleEmptied();
            
        }
    }
}