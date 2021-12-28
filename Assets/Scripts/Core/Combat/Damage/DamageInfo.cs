using System;

namespace Core.Combat.Damage
{
    [Serializable]
    public struct DamageInfo
    {
        public float healthDamage;
        public float hypnoDamage;

        public static DamageInfo NormalAttack(float damage = 10)
        {
            return new DamageInfo() {healthDamage = damage};
        }
        
        public static DamageInfo HypnoAttack(float damage= 10)
        {
            return new DamageInfo() {hypnoDamage = damage};
        }

        public override string ToString()
        {
            return $"healthDamage: {healthDamage}, hypnoDamage: {hypnoDamage} ";
        }
    }
}