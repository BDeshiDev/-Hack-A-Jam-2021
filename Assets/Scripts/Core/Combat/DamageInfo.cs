using System;

namespace Core.Combat
{
    [Serializable]
    public struct DamageInfo
    {
        public float rawDamage;
        public float hypnoDamage;

        public static DamageInfo NormalAttack()
        {
            return new DamageInfo() {rawDamage = 10};
        }
        
        public static DamageInfo HypnoAttack()
        {
            return new DamageInfo() {hypnoDamage = 10};
        }
    }
}