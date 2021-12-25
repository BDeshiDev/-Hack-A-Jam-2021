using System;
using BDeshi.BTSM;
using Core.Combat;
using Core.Physics;
using UnityEngine;

namespace Core.Player
{
    [RequireComponent(typeof(MoveComponent))]
    public class HypnoPlayer: BlobEntity
    {
        [SerializeField] Transform shootIndicator;
        [SerializeField] PlayerIdleState idleState;
        [SerializeField] PlayerChargableAttackState chargableAttackState;
        [SerializeField] PlayerDashState dashState;
        public override TargetResolverComponent TargetResolverComponent => playerTargetter;
        TargetResolverComponent playerTargetter;
        

        public override void look(Vector3 dir, Vector3 aimPoint)
        {
            base.look(dir, aimPoint);
            shootIndicator.position = aimPoint;
        }
        
        public override EventDrivenStateMachine<Events> createFSM()
        {
            EventDrivenStateMachine<Events> fsm = new EventDrivenStateMachine<Events>(idleState);
            
            fsm.addTransition(dashState, idleState,()=> dashState.IsComplete);
            fsm.addTransition(chargableAttackState, idleState,()=> chargableAttackState.IsComplete);
            
            fsm.addEventTransition(idleState,Events.MeleeChargeStart, chargableAttackState );

            fsm.addGlobalEventTransition(Events.Dash, dashState);
            fsm.addGlobalEventTransition(Events.MeleeChargeStart, chargableAttackState );

            fsm.addEventHandler(chargableAttackState, Events.MeleeChargeRelease, chargableAttackState.handleChargeReleased);

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
            fsm.handleEvent(Events.MeleeChargeStart);
        }

        public void handleMeleeReleased()
        {
            fsm.handleEvent(Events.MeleeChargeRelease);
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