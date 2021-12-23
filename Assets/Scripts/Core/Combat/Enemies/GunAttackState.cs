using BDeshi.Utility;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public class GunAttackState : AttackState
    {
        [SerializeField] Gun gun;
        [SerializeField] FiniteTimer postShotWaitTimer = new FiniteTimer(1.8f);
        public override void EnterState()
        {
            postShotWaitTimer.reset();
            gun.shoot();
        }

        public override void Tick()
        {
            postShotWaitTimer.safeUpdateTimer(Time.deltaTime);
        }

        public override void ExitState()
        {
            
        }

        public override bool IsComplete => postShotWaitTimer.isComplete;
    }
}