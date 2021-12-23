using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public  class PrepAttackState: EnemyStatebase
    {
        [SerializeField] float range = 4;
        public FiniteTimer timer = new FiniteTimer(2);
        public bool wasInRangeLastFrame = false;
        [SerializeField] bool lookAtTarget = true;

        public override void EnterState()
        {
            timer.reset();
            wasInRangeLastFrame = false;
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
                wasInRangeLastFrame = true;
            }

            timer.safeUpdateTimer(Time.deltaTime);
        }

        public override void ExitState()
        {
            wasInRangeLastFrame = false;
        }
    }
}