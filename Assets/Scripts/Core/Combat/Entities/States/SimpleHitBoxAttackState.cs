using BDeshi.Utility;
using Core.Combat.Damage.Hitbox;
using UnityEngine;

namespace Core.Combat.Entities.States
{
    public class SimpleHitBoxAttackState : AttackState
    {
        public SpriteRenderer spriter;
        [SerializeField] private RectangleHitBox hitbox;
        [SerializeField] private FiniteTimer attackTimer = new FiniteTimer(0, .2f);
        public override bool IsComplete => attackTimer.isComplete;
        private bool running = false;
        private void Awake()
        {
            attackTimer.complete();
        }


        public override void EnterState()
        {
            attackTimer.reset();
            spriter.enabled = true;
            hitbox.startDetection();
            
            // Debug.Log("enter" + gameObject);
            running = true;
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

            // Debug.Log("exit"+ gameObject);
            running = false;

        }
    }
}

