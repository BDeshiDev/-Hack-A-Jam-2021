using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat.Entities.States.EnemyState
{
    public  class PrepAttackState: EnemyStatebase
    {
        [SerializeField] float range = 4;
        public FiniteTimer maxTimer = new FiniteTimer(2);
        public FiniteTimer minTimer = new FiniteTimer(.33f);
        public bool WasInRangeLastFrame = false;
        [SerializeField] bool lookAtTarget = true;
        [SerializeField] UnityEvent OnPrepStart;
        [SerializeField] UnityEvent OnPrepTick;
        [SerializeField] UnityEvent OnPrepFinish;
        public float moveSpeedModifier = 1.67f;
        public bool IsComplete => maxTimer.isComplete || (WasInRangeLastFrame && minTimer.isComplete);
        public bool IsTimeout =>  maxTimer.isComplete;
        public override void EnterState()
        {
            maxTimer.reset();
            minTimer.reset();
            WasInRangeLastFrame = false;
            OnPrepStart.Invoke();
        }

        public override void Tick()
        {
            var vecToTarget = (entity.TargetResolverComponent.getTargetPos() - entity.transform.position);
            if (vecToTarget.exceedSqrDist(range))
            {
                entity.MoveComponent.moveInputThisFrame = vecToTarget.normalized * moveSpeedModifier;
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
            
            OnPrepTick.Invoke();
        }

        public override void ExitState()
        {
            OnPrepFinish.Invoke();
            WasInRangeLastFrame = false;
        }
    }
}