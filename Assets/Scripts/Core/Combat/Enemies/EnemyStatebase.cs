using BDeshi.BTSM;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public abstract class EnemyStatebase : MonoBehaviourStateBase
    {
        [SerializeField] protected EnemyEntity entity;
    }
}