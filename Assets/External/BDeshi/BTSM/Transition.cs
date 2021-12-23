using System;
using JetBrains.Annotations;

namespace BDeshi.BTSM
{
    /// <summary>
    /// General interface for Transitions
    /// </summary>
    public interface Transition
    {
        /// <summary>
        /// Returns null if evaluation fails
        /// </summary>
        /// <returns></returns>
        bool Evaluate();
        
        State SuccessState { get; }
        bool TakenLastTime { get; set; }
        [CanBeNull]public Action OnTaken { get; }

    }
}