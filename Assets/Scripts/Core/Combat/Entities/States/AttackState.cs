using BDeshi.BTSM;
using UnityEngine;

namespace Core.Combat.Entities.States
{
    public abstract class AttackState : MonoBehaviourStateBase
    {
        [SerializeField] protected BlobEntity blobEntity;
        public abstract bool IsComplete { get; }
        // public abstract bool CanBeCancelled { get; }
    }
}