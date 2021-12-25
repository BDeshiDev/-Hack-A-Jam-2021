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
        //tested melee but it was too slow for the player.
        [SerializeField] PlayerChargableAttackState chargableAttackState;
        [SerializeField] PlayerDashState dashState;
        public override TargetResolverComponent TargetResolverComponent => playerTargetter;
        TargetResolverComponent playerTargetter;
        
        public GunAttackState playerGunState;
        
        

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
            fsm.addEventTransition(idleState, Events.Attack1Held, playerGunState);
            fsm.addTransition(playerGunState, idleState, () => playerGunState.IsComplete);
            fsm.addEventHandler(playerGunState, Events.Attack1Held,
                () => { fsm.transitionTo(playerGunState, true, true); }
                );
            return fsm;
        }


        protected override void Awake()
        {
            base.Awake();
            playerTargetter = GetComponent<PlayerTargetResolver>();
        }


        private void OnEnable()
        {
            healthComponent.Emptied += handleDeath;
        }
        
        private void OnDisable()
        {
            healthComponent.Emptied -= handleDeath;
        }

        private void handleDeath(ResourceComponent obj)
        {
            GameStateManager.Instance.handleEvent(GameStateManager.Event.Gameover);
        }


        public void handleMeleeHeld()
        {
            fsm.handleEvent(Events.Attack1Held);
        }

        public void handleMeleeReleased()
        {
            fsm.handleEvent(Events.Attack1Release);
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