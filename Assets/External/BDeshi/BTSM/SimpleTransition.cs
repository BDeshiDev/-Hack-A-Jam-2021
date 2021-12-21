using System;

namespace BDeshi.BTSM
{
    public class SimpleTransition: Transition
    {
        private State s;
        private Func<bool> evaluateFunc;
        
        /// <summary>
        /// Create a transition to a state
        /// </summary>
        /// <param name="s"></param>
        /// <param name="evaluateFunc">If NULL Transition will ALWAYS BE TRUE</param>
        public SimpleTransition(State s, Func<bool> evaluateFunc = null)
        {
            this.s = s;
            this.evaluateFunc = evaluateFunc;
        }

        public State SuccessState => s;
        public bool TakenLastTime { get; set; }

        /// <summary>
        /// If Func return true, else return false
        /// </summary>
        /// <returns></returns>
        bool Transition.Evaluate()
        {
            if (evaluateFunc == null || evaluateFunc.Invoke())
                return true;
            return false;
        }
    }
}