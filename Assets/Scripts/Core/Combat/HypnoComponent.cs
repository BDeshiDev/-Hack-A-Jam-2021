using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Combat
{
    public class HypnoComponent: ResourceComponent
    {
        public float hypnosisRecoveryRate = 1;
        
        private void Update()
        {
            reduceAmount(Time.deltaTime * hypnosisRecoveryRate);
        }
    }
}