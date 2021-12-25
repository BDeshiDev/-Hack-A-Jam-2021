using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Combat
{

    [Serializable]
    public class SpawnSlot
    {
        public EnemyEntity prefab;
        public int count;
        public int max = 2;
        public int min = 1;
        public List<EnemyEntity> spawned = new List<EnemyEntity>();

        public void setCount(ref int remainingTotalCount)
        {
            count = Random.Range(min, Mathf.Min(max + 1, remainingTotalCount));
            remainingTotalCount-= count;
        }

        public void startSpawn(Spawner spawner)
        {
            for (int i = 0; i < count; i++)
            {
                var summoningCircle = GameplayPoolManager.Instance.summoningCircles.get(spawner.summoningCirclePrefab);
                spawner.StartCoroutine(
                    summoningCircle.summon(spawner, prefab, spawned)
                );
            }
        

        }
    }
    
}