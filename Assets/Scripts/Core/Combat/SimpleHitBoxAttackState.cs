using System;
using BDeshi.Utility;
using UnityEngine;

namespace Core.Combat
{
    public class SimpleHitBoxAttackState : AttackState
    {
        public SpriteRenderer spriter;
        [SerializeField] private HitBox hitbox;
        [SerializeField] private FiniteTimer attackTimer = new FiniteTimer(0, .2f);
        public override bool IsComplete => attackTimer.isComplete;
        
        private void Awake()
        {
            attackTimer.complete();
        }


        public override void EnterState()
        {
            attackTimer.reset();
            spriter.enabled = true;
            hitbox.startDetection();
        }

        public override void Tick()
        {
            if (!attackTimer.isComplete)
            {
                attackTimer.updateTimer(Time.deltaTime);

            }
        }

        public override void ExitState()
        {
            spriter.enabled = false;
            hitbox.stopDetection();
        }
    }
}

