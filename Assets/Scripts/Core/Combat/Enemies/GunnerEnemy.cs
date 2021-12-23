using System;
using BDeshi.BTSM;
using BDeshi.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Combat.Enemies
{
    public class GunnerEnemy : EnemyEntity
    {
        [SerializeField] MaintainRangeState maintainRangeState;
        [SerializeField] PrepAttackState prepAttackState;
        [SerializeField] GunAttackState gunAttackState;
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
            
            fsm.addTransition(prepAttackState, gunAttackState,()=> prepAttackState.IsComplete);
            fsm.addTransition(gunAttackState, maintainRangeState,
                ()=> gunAttackState.IsComplete,
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