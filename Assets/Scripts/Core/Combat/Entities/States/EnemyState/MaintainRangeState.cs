using BDeshi.BTSM;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat.Enemies
{
    public  class MaintainRangeState: EnemyStatebase
    {
        [FormerlySerializedAs("range")] [SerializeField] float minRange = 8;
        [FormerlySerializedAs("range")] [SerializeField] float maxrange = 8;
        [SerializeField] private float backOffSpeedMultiplier = .7f;
        [SerializeField] private float forwardSpeedMultiplier = 1f;
        [SerializeField] bool lookAtTarget = true;
        public FiniteTimer distChangeTimer = new FiniteTimer(.5f);
        [SerializeField]private Vector3 curDir;
        public override void EnterState()
        {
            distChangeTimer.reset();
        }

        public override void Tick()
        {
            var vecToTarget = (entity.TargetResolverComponent.getTargetPos() - entity.transform.position);
            if(distChangeTimer.tryCompleteTimer(Time.deltaTime))
            {
                distChangeTimer.reset();
                if (!vecToTarget.exceedSqrDist(minRange))
                {
                    curDir = vecToTarget.normalized * forwardSpeedMultiplier;
                }
                else if(vecToTarget.exceedSqrDist(maxrange))
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