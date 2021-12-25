using BDeshi.BTSM;
using BDeshi.Utility.Extensions;
using Core.Physics;
using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Do basic blob stuff, logic driven via a FSM.
    /// Sufficient for both player and enemies for the duration of the jam
    /// </summary>
    public abstract class BlobEntity : CombatEntity
    {
        [SerializeField] protected SpriteRenderer spriter;
        public FSMRunner fsmRunner;
        public MoveComponent MoveComponent { get; protected set; }
        [SerializeField] Transform aimer;
        protected EventDrivenStateMachine<Events> fsm;
        public Vector2 LastLookDir { get; private set; }
        public abstract EventDrivenStateMachine<Events> createFSM();

        protected override void Awake()
        {
            base.Awake();
            fsmRunner = GetComponent<FSMRunner>();
            MoveComponent = GetComponent<MoveComponent>();
        }
        public virtual void look(Vector3 dir, Vector3 point)
        {
            aimer.allignToDir(dir);
            LastLookDir = aimer.right;
        }
        public virtual void lookAlong(Vector3 dir)
        {
            aimer.allignToDir(dir);
            LastLookDir = aimer.right;

        }
        
        
        protected virtual void Start()
        {
            initializeFSM();
        }
        
        public void initializeFSM()
        {
            if(fsm == null)
                fsm = createFSM();
            fsmRunner.Initialize(fsm);
        }

        public enum Events
        {
            //playerside
            MeleeChargeStart,
            MeleeChargeRelease,
            //EnemySide,
            
            
            //global
            // Hurt,
            // Death,
            Dash,
            Berserk
        }
    }
}