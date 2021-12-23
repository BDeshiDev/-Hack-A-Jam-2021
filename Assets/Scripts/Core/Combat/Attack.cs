﻿using UnityEngine;

namespace Core.Combat
{
    public abstract class Attack : MonoBehaviour
    {
        public abstract bool IsAttackComplete { get; }
        public abstract void startAttack();
        protected abstract void stopAttack();
    }
}