﻿using System;
using System.Timers;
using BDeshi.BTSM;
using BDeshi.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Core.Combat.Enemies
{
    public class GunnerEnemy : EnemyEntity
    {
        [SerializeField] MaintainRangeState normalMaintainRangeState;
        [SerializeField] PrepAttackState normalPrepAttackState;
        [SerializeField] AttackState normalAttackState;
        [SerializeField] MaintainRangeState berserkMaintainRangeState;
        [SerializeField] PrepAttackState berserkPrepAttackState;
        [SerializeField] AttackState berserkAttackState;



        public override EventDrivenStateMachine<Events> createFSM()
        {
            fsm = new EventDrivenStateMachine<Events>(normalMaintainRangeState);
            
            setBasicUpStates(normalMaintainRangeState, normalPrepAttackState, normalAttackState);
            setBasicUpStates(berserkMaintainRangeState, berserkPrepAttackState, berserkAttackState);
            
            
            fsm.addGlobalEventTransition(Events.Berserk, berserkMaintainRangeState);
            return fsm;
        }
        //flow of fsm is same for berserk and nonberserk states
        //only diff is the values
        //which is tied to different state instances
        void setBasicUpStates(MaintainRangeState maintainRangeState,PrepAttackState prepAttackState, AttackState attackState)
        {
            fsm.addTransition(maintainRangeState, prepAttackState,
                () =>  attackCoolDown.isComplete);
            
            fsm.addTransition(prepAttackState, attackState,()=> prepAttackState.IsComplete);
            fsm.addTransition(attackState, maintainRangeState,
                ()=> attackState.IsComplete,
                () =>
                {
                    attackCoolDown.reset();
                });
        }
    }
}