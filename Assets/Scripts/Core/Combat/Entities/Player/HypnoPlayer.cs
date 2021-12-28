using System;
using BDeshi.BTSM;
using Core.Combat;
using Core.Combat.Enemies;
using Core.Physics;
using UnityEngine;

namespace Core.Player
{
    [RequireComponent(typeof(MoveComponent))]
    public class HypnoPlayer: BlobEntity
    {
        [SerializeField] Transform shootIndicator;
        [SerializeField] PlayerIdleState idleState;
        [SerializeField] GunAttackState playerGunState;

        //tested melee but it was too slow for the player.
        [SerializeField] PlayerChargableAttackState chargableAttackState;
        [SerializeField] PlayerDashState dashState;
        public override TargetResolverComponent TargetResolverComponent => playerTargetter;
        TargetResolverComponent playerTargetter;
        public GunAttackState PlayerGunState =>playerGunState;


        public override void look(Vector3 dir, Vector3 aimPoint)
        {
            base.look(dir, aimPoint);
            shootIndicator.position = aimPoint;
        }
        
        public override EventDrivenStateMachine<Events> createFSM()
        {
            EventDrivenStateMachine<Events> fsm = new EventDrivenStateMachine<Events>(idleState);
            
            fsm.addTransition(dashState, idleState,()=> dashState.IsComplete);
            

            fsm.addGlobalEventTransition(Events.Dash, dashState);
            
            // disabled melee
            // fsm.addTransition(chargableAttackState, idleState,()=> chargableAttackState.IsComplete);
            // fsm.addEventTransition(idleState,Events.Attack1Held, chargableAttackState );
            // fsm.addGlobalEventTransition(Events.Attack1Held, chargableAttackState );
            
            // fsm.addEventHandler(chargableAttackState, Events.Attack1Release, chargableAttackState.handleChargeReleased);
            
            //can shoot from any state except dash
            fsm.addTransition(playerGunState, idleState, () => playerGunState.IsComplete);
            
            fsm.addEventHandler(playerGunState, Events.Attack1Held, shootTransition);
            fsm.addEventHandler(idleState, Events.Attack1Held, shootTransition);
            return fsm;
        }


        protected override void Awake()
        {
            base.Awake();
            playerTargetter = GetComponent<PlayerTargetResolver>();
        }

        public void shootTransition()
        {
            if(playerGunState.Gun.CanFire)
                fsm.transitionTo(playerGunState, true, true);
        }

        private void OnEnable()
        {
            healthComponent.Emptied += handleDeath;
        }
        
        private void OnDisable()
        {
            healthComponent.Emptied -= handleDeath;
        }
        [ContextMenu("FORCE DIE")]
        private void handleDeath()
        {
            if(!CanDie)
                return;
            invokeDeathEvent();
            GameStateManager.Instance.handleEvent(GameStateManager.Event.Gameover);
        }  
        
        private void handleDeath(ResourceComponent obj)
        {
            handleDeath();
        }


        public void handleMeleeHeld()
        {
            fsm.handleEvent(Events.Attack1Held);
        }

        public void handleMeleeReleased()
        {
            fsm.handleEvent(Events.Attack1Release);
        }

        public override void takeDamage(DamageInfo damage)
        {
            base.takeDamage(damage);
            if (isInvulnerable)//since dash = only source on invincibility, this means we have dodge through attack
            {
                CombatEventManger.Instance.OnSuccessFullDodge.Invoke();
            }
        }


        public void handleDashHeld()
        {
            fsm.handleEvent(Events.Dash);
        }

        protected override void Start()
        {
            base.Start();
            healthComponent.fullyRestore();
        }
    }
}