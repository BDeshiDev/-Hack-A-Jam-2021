using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat.Enemies
{
    public  class PrepAttackState: EnemyStatebase
    {
        [SerializeField] float range = 4;
        public FiniteTimer maxTimer = new FiniteTimer(2);
        public FiniteTimer minTimer = new FiniteTimer(.33f);
        public bool WasInRangeLastFrame = false;
        [SerializeField] bool lookAtTarget = true;
        [SerializeField] UnityEvent OnPrepStart;
        [SerializeField] UnityEvent OnPrepFinish;
        public bool IsComplete => maxTimer.isComplete || (WasInRangeLastFrame && minTimer.isComplete);
        public override void EnterState()
        {
            maxTimer.reset();
            minTimer.reset();
            WasInRangeLastFrame = false;
            OnPrepStart.Invoke();
        }

        public override void Tick()
        {
            var vecToTarget = (entity.target.position - entity.transform.position);
            if (vecToTarget.exceedSqrDist(range))
            {
                entity.MoveComponent.moveInputThisFrame = vecToTarget.normalized;
                WasInRangeLastFrame = false;
                minTimer.reset();
            }
            else
            {
                WasInRangeLastFrame = true;
                minTimer.safeUpdateTimer(Time.deltaTime);
            }

            if (lookAtTarget)
            { 
                entity.lookAlong(vecToTarget);
            }

            maxTimer.safeUpdateTimer(Time.deltaTime);
        }

        public override void ExitState()
        {
            OnPrepFinish.Invoke();
            WasInRangeLastFrame = false;
        }
    }
}