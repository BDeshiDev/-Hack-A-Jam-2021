using UnityEngine;

namespace Core.Combat
{
    public abstract class Attack : MonoBehaviour
    {
        public abstract bool IsAttackComplete { get; }
        public abstract void startAttack();
        public abstract void stopAttack();
    }
}