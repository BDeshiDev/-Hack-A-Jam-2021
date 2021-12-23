using System;
using BDeshi.Utility;
using UnityEngine;

namespace Core.Combat
{
    public class SimpleHitBoxAttack : Attack
    {
        public SpriteRenderer spriter;
        [SerializeField] private HitBox hitbox;
        [SerializeField] private FiniteTimer attackTimer = new FiniteTimer(0, .2f);
        public override bool IsAttackComplete => attackTimer.isComplete;
        
        private void Awake()
        {
            attackTimer.complete();
        }

        public override void startAttack()
        {
            attackTimer.reset();
            spriter.enabled = true;
            hitbox.startDetection();
        }


        protected override void stopAttack()
        {
            spriter.enabled = false;
            hitbox.stopDetection();
        }

        private void Update()
        {
            if (attackTimer.isComplete)
            {
                return;
            }
            else
            {
                attackTimer.updateTimer(Time.deltaTime);
                if (attackTimer.isComplete)
                {
                    stopAttack();
                }
            }
        }
    }
}

