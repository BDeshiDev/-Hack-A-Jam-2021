using BDeshi.BTSM;
using UnityEngine;

namespace Core.Combat
{
    public abstract class AttackState : MonoBehaviourStateBase
    {
        public abstract bool IsComplete { get; }
    }
}