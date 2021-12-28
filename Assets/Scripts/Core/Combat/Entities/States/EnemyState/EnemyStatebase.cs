using BDeshi.BTSM;
using Core.Combat.Entities.Enemies;
using UnityEngine;

namespace Core.Combat.Entities.States.EnemyState
{
    public abstract class EnemyStatebase : MonoBehaviourStateBase
    {
        [SerializeField] protected EnemyEntity entity;
    }
}