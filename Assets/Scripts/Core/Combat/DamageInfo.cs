using System;

namespace Core.Combat
{
    [Serializable]
    public struct DamageInfo
    {
        public float healthDamage;
        public float hypnoDamage;

        public static DamageInfo NormalAttack()
        {
            return new DamageInfo() {healthDamage = 10};
        }
        
        public static DamageInfo HypnoAttack()
        {
            return new DamageInfo() {hypnoDamage = 10};
        }

        public override string ToString()
        {
            return $"healthDamage: {healthDamage}, hypnoDamage: {hypnoDamage} ";
        }
    }
}