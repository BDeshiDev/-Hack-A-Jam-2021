using System;
using BDeshi.BTSM;
using BDeshi.Utility;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public class GunnerEnemy : EnemyEntity
    {
        [SerializeField] MaintainRangeState maintainRangeState;
        [SerializeField] PrepAttackState prepAttackState;
        public FiniteTimer attackCoolDown = new FiniteTimer(0, 2f);
        public bool canRecoverCooldown = true;
        public override StateMachine createFSM()
        {
            EventDrivenStateMachine<String> fsm = new EventDrivenStateMachine<string>(maintainRangeState);
            
            fsm.addTransition(maintainRangeState, prepAttackState,
                                () => prepAttackState.WasInRangeLastFrame || prepAttackState.timer.isComplete,
                                () => { 
                                    attackCoolDown.reset();
                                    canRecoverCooldown = false;
                                });
            
            
            
            return fsm;
        }
    }

    public class AttackState: EnemyStatebase
    {
        [SerializeField] private Attack attack;
        public override void EnterState()
        {
            attack.startAttack();
        }

        public override void Tick()
        {
            
        }

        public override void ExitState()
        {
            attack.stopAttack();
        }
    }
}