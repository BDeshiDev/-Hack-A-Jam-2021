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
        
        public override StateMachine createFSM()
        {
            EventDrivenStateMachine<String> fsm = new EventDrivenStateMachine<string>(maintainRangeState);
            
            fsm.addTransition(maintainRangeState, prepAttackState,
                        () => prepAttackState.wasInRangeLastFrame || prepAttackState.timer.isComplete,
                        () => attackCoolDown.reset());
            
            
            
            return fsm;
        }
    }
}