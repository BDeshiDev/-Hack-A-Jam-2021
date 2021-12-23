using System;
using UnityEngine;

namespace BDeshi.BTSM
{
    public class BTCompleteTransition : Transition
    {
        private BtNodeBase node;
        public State SuccessState => successState;
        public bool TakenLastTime { get; set; }
        public Action OnTaken { get; }
        
        public State successState;

        public BTCompleteTransition(BtNodeBase node, State state)
        {
            this.node = node;
            this.successState = state;
            // Debug.Log("??" + (successState == null?"nnnnnulll":successState.Name));
        }

        public bool Evaluate()
        {
            if (node.LastStatus == BTStatus.Success)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{node.Name}.complete->{SuccessState.FullStateName}";
        }
    }
}