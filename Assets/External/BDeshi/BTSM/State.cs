using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace BDeshi.BTSM
{
    public interface State
    {
        void EnterState();
        void Tick();
        void ExitState();
        string Prefix { get; set; }
        string FullStateName { get; }
        string Name{ get; }
        [CanBeNull] State Parent { get; set; }
    }
}