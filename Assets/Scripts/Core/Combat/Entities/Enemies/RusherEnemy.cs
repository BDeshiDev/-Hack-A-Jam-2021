using BDeshi.BTSM;
using Core.Combat.Entities.States;
using Core.Combat.Entities.States.EnemyState;
using UnityEngine;

namespace Core.Combat.Entities.Enemies
{
    //requires one more state for moving back before charge
    //ultimately better to keep a separate class like this
    //because a more fleshed out enemy would have more differences anyways
    public class RusherEnemy : EnemyEntity
    {
        [SerializeField] MaintainRangeState normalMaintainRangeState;
        [SerializeField] PrepAttackState normalPrepAttackState;
        [SerializeField] PrepAttackState preAttackBackoffState;
        [SerializeField] AttackState normalAttackState;
        
        [SerializeField] MaintainRangeState berserkMaintainRangeState;
        [SerializeField] PrepAttackState berserkPrepAttackState;
        [SerializeField] PrepAttackState berserkPreAttackBackoffState;
        [SerializeField] AttackState berserkAttackState;



        public override EventDrivenStateMachine<Events> createFSM()
        {
            fsm = new EventDrivenStateMachine<Events>(normalMaintainRangeState);
            
            setBasicUpStates(normalMaintainRangeState, normalPrepAttackState,preAttackBackoffState, normalAttackState);
            setBasicUpStates(berserkMaintainRangeState, berserkPrepAttackState,berserkPreAttackBackoffState, berserkAttackState);
            
            
            fsm.addGlobalEventTransition(Events.Berserk, berserkMaintainRangeState);
            return fsm;
        }

        void setBasicUpStates(MaintainRangeState maintainRangeState,PrepAttackState prepAttackState,PrepAttackState preAttackBackoffState, AttackState attackState)
        {
            fsm.addTransition(maintainRangeState, prepAttackState,
                () =>  attackCoolDown.isComplete);
            
            fsm.addTransition(prepAttackState, preAttackBackoffState,()=> prepAttackState.IsComplete);
            fsm.addTransition(preAttackBackoffState, attackState,()=> preAttackBackoffState.IsTimeout);
            fsm.addTransition(attackState, maintainRangeState,
                ()=> attackState.IsComplete,
                () =>
                {
                    attackCoolDown.reset();
                });
        }
    }
}