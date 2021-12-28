using System;
using Core.Combat.Entities.Enemies;
using Core.Combat.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Combat.Spawning
{

    [Serializable]
    public class SpawnSlot
    {
        public EnemyEntity prefab;
        public int count;
        public int max = 2;
        public int min = 1;

        public void setCount(ref int remainingTotalCount)
        {
            count = Mathf.Min(remainingTotalCount,Random.Range(min, max + 1));
            remainingTotalCount-= count;
        }

        public void startSpawn(Spawner spawner)
        {
            for (int i = 0; i < count; i++)
            {

                var summoningCircle = GameplayPoolManager.Instance.summoningCircles.get(spawner.summoningCirclePrefab);
                summoningCircle.startSummon(prefab, spawner.findSafeSpawnSpot(), spawner.trackEnemy);
            }
        }
    }
    
}