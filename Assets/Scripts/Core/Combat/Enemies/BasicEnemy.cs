using System;
using BDeshi.BTSM;
using BDeshi.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Core.Combat.Enemies
{
    public class BasicEnemy : EnemyEntity
    {
        [SerializeField] MaintainRangeState maintainRangeState;
        [SerializeField] PrepAttackState prepAttackState;
        [SerializeField] AttackState attackState;
        public FiniteTimer attackCoolDown = new FiniteTimer(0, 2f);
        public bool canRecoverCooldown = true;
        public override EventDrivenStateMachine<Events> createFSM()
        {
            EventDrivenStateMachine<Events> fsm = new EventDrivenStateMachine<Events>(maintainRangeState);
            
            fsm.addTransition(maintainRangeState, prepAttackState,
                                () =>  attackCoolDown.isComplete,
                                () => { 
                                    attackCoolDown.reset();
                                    canRecoverCooldown = false;
                                });
            
            fsm.addTransition(prepAttackState, attackState,()=> prepAttackState.IsComplete);
            fsm.addTransition(attackState, maintainRangeState,
                ()=> attackState.IsComplete,
                () =>
                {
                    canRecoverCooldown = true;
                });
            
            return fsm;
        }

        void Update()
        {
            if (canRecoverCooldown)
            {
                attackCoolDown.safeUpdateTimer(Time.deltaTime);
            }

        }
    }
}