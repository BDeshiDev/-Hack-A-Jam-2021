using BDeshi.BTSM;
using BDeshi.Utility.Extensions;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public  class MaintainRangeState: EnemyStatebase
    {
        [SerializeField] float range = 8;
        [SerializeField] private float backOffSpeedMultiplier = .7f;
        [SerializeField] bool lookAtTarget = true;
        public override void EnterState()
        {
               
        }

        public override void Tick()
        {
            var vecToTarget = (entity.target.position - entity.transform.position);
            if (vecToTarget.exceedSqrDist(range))
            {
                entity.moveComponent.moveInputThisFrame = vecToTarget.normalized;
            }
            else
            {
                entity.moveComponent.moveInputThisFrame = -vecToTarget.normalized * backOffSpeedMultiplier;
            }

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