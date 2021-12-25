using UnityEngine;

namespace BDeshi.BTSM
{
    public class FSMRunner: MonoBehaviour
    {
        public StateMachine fsm;
        /// <summary>
        /// Calls fsm.enter
        /// </summary>
        public  void Initialize(StateMachine fsm, bool callEnter = true)
        {
            this.fsm = fsm;
            fsm.DebugContext = gameObject;
            fsm.enter(callEnter);
        }
        

        
        /// <summary>
        /// Just ticks FSM.
        /// </summary>
        protected virtual void Update()
        {
            fsm.Tick();
        }

    }
}