using BDeshi.BTSM;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public  class MaintainRangeState: EnemyStatebase
    {
        [SerializeField] float range = 8;
        [SerializeField] private float backOffSpeedMultiplier = .7f;
        [SerializeField] bool lookAtTarget = true;
        public FiniteTimer distChangeTimer = new FiniteTimer(.5f);
        private Vector3 curDir;
        public override void EnterState()
        {
            distChangeTimer.reset();
        }

        public override void Tick()
        {
            var vecToTarget = (entity.target.position - entity.transform.position);
            if(distChangeTimer.tryCompleteTimer(Time.deltaTime))
            {
                distChangeTimer.reset();
                if (vecToTarget.exceedSqrDist(range))
                {
                    curDir = vecToTarget.normalized;
                }
                else
                {
                    curDir = -vecToTarget.normalized * backOffSpeedMultiplier;
                }
            }
            entity.MoveComponent.moveInputThisFrame = curDir;
            if (lookAtTarget)
            {
                entity.lookAlong(vecToTarget);
            }
        }

        public override void ExitState()
        {
            
        }
    }
}