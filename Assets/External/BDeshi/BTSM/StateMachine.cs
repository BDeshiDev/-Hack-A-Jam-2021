using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace BDeshi.BTSM
{
    public class StateMachine
    {
        public State curState;
        public State startingState;
        public GameObject DebugContext = null;

        /// <summary>
        /// Transitions list for this state
        /// </summary>
        public List<Transition> activeTransitions { get; protected set; }

        /// <summary>
        /// This is there in case current state does not have transitions
        /// and so that we don't have to create a new list
        /// </summary>
        public static readonly List<Transition> emptyTransitions =new List<Transition>();
        /// <summary>
        /// Transitions from a state to another
        /// </summary>
        public Dictionary<State, List<Transition>> transitions { get; protected set; } = new Dictionary<State, List<Transition>>();
        /// <summary>
        /// Transitions that are always active
        /// </summary>
        public List<Transition> globalTransitions { get; protected set; }= new List<Transition>();
        public StateMachine(State startingState)
        {
            this.startingState = startingState;
        }

        public void enter(bool callEnter = true)
        {
            transitionTo(startingState, callEnter);
        }
        
        public void exitCurState()
        {
            State cur = curState;
            while (cur != null)
            {
                cur.ExitState();
                cur = cur.Parent;
            }
        }

        public void Tick()
        {
            curState.Tick();

            State newState = null;
            foreach (var activeTransition in activeTransitions)
            {
                if (activeTransition.Evaluate())
                {
                    newState = activeTransition.SuccessState;
                    activeTransition.TakenLastTime = true;
// #if DEBUG
//                     if (DebugContext)
//                         Debug.Log("from " + (curState.FullStateName ) + "To " + activeTransition, DebugContext);
// #endif
                    activeTransition.OnTaken?.Invoke();
                    break;
                }
                else
                {
                    activeTransition.TakenLastTime = false;
                }
            }

            if (newState == null)
            {
                foreach (var activeTransition in globalTransitions)
                {
                    if (activeTransition.Evaluate())
                    {
                        newState = activeTransition.SuccessState;                    
                        activeTransition.TakenLastTime = true;

#if DEBUG
                        if (DebugContext)
                            Debug.Log("from " + (curState.FullStateName) + "To " + activeTransition, DebugContext);
#endif
                        break;
                    }
                    else
                    {
                        activeTransition.TakenLastTime = false;
                    }
                }
            }

            transitionTo(newState);
        }

        /// <summary>
        /// Transitions to a state given that it is null
        /// Calls oldstate.exit() if it is not null
        /// Then sets up newState via newState.enter()
        /// Handles recursion.
        /// </summary>
        /// <param name="newState">
        /// Limit this to states this Dictionary knows about. Otherwise, the Actions/Transitions will not work
        /// </param>
        /// <param name="callEnter">
        /// If true, call the enter function in the state(s) transitioned to
        /// Usecase: initialize curState without calling enter
        /// </param>
        public void transitionTo(State newState, bool callEnter = true, bool forceEnterIfSameState = false)
        {
            if (newState != null && (newState != curState || forceEnterIfSameState))
            {

                // if (DebugContext)
                //     Debug.Log("from " +(curState == null?"null": curState.FullStateName)  + "To " + newState.FullStateName, DebugContext);

                if(callEnter)
                    recursiveTransitionToState(newState);
                else
                {
                    curState = newState;
                }

                HandleTransitioned();
            }
        }
        /// <summary>
        /// Set transitions list to curState's.
        /// </summary>
        protected virtual void HandleTransitioned()
        {
            if (transitions.TryGetValue(curState, out var newTransitionsList))
            {
                activeTransitions = newTransitionsList;
            }
            else
            {
                activeTransitions = emptyTransitions;
            }
        }


        void recursiveTransitionToState(State to)
        {
            var cur = curState;

            State commonParent = null;
            while (cur != null)
            {
                var toParent = to;
                while (toParent != null)
                {
                    if (toParent == cur)
                    {
                        commonParent = cur;
                        break;
                    }
                    toParent = toParent.Parent;
                }
                

                if (commonParent != null)
                    break;
                
                cur.ExitState();
                cur = cur.Parent;
            }
            // Debug.Log( curState?.FullStateName + "->"+ to?.FullStateName+" to "  +commonParent?.FullStateName);
            callEnterRecursive(to, commonParent);
            curState = to;
        }
        /// <summary>
        /// Recurse to some parent and call enterstate of all childs recursively down to the passed one
        /// </summary>
        /// <param name="child"> The child we start recursing from. DO NOT MAKE THIS == PARENT</param>
        /// <param name="limitParent">The parent we won't call enter on. </param>
        void callEnterRecursive(State child, [CanBeNull] State limitParent)
        {
            if(child == null || child == limitParent)
                return;

            callEnterRecursive(child.Parent, limitParent);
            child.EnterState();
        }

        public void addTransition(State from, Transition t)
        {
            if(transitions.TryGetValue(from, out var l))
                l.Add(t);
            else
            {
                transitions.Add(from, new List<Transition>(){t});
            }
        }
        
        public void addTransition(State from, State to, Func<bool> condition, Action onTaken = null )
        {
            addTransition(from, new SimpleTransition(to, condition, onTaken));
        }

        public void addGlobalTransition(Transition t)
        {
            globalTransitions.Add(t);
        }

        public void cleanup()
        {
            if (curState != null)
            {
                curState.ExitState();
            }
        }
    }
}