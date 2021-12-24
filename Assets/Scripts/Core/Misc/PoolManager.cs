using System;
using System.Collections.Generic;
using bdeshi.utility;
using BDeshi.Utility;
using Core.Combat;
using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Single place for pools. Ugly but sufficient for the jam
    /// </summary>
    public class PoolManager: MonoBehaviourLazySingleton<PoolManager>
    {
        public PrefabPoolPool<Projectile> projectilePool;
        public PrefabPoolPool<EnemyEntity> enemyPool;
        public PrefabPoolPool<SummoningCircle> summoningCircles;

        protected override void initialize()
        {
            base.initialize();
            projectilePool = new PrefabPoolPool<Projectile>(20, new GameObject("Projectiles").transform);
            enemyPool = new PrefabPoolPool<EnemyEntity>(3, new GameObject("Enemies").transform);
            summoningCircles = new PrefabPoolPool<SummoningCircle>(1, new GameObject("summingCircles").transform);
        }
    }
    
    /// <summary>
    /// search by prefab
    /// </summary>
    /// <typeparam name="TPrefab"></typeparam>
    public class PrefabPoolPool<TPrefab> where TPrefab: MonoBehaviour, AutoPoolable<TPrefab>
    {
        public Dictionary<TPrefab, AutoMonobehaviourPool<TPrefab>> pools 
            = new Dictionary<TPrefab, AutoMonobehaviourPool<TPrefab>>();

        private int initialCount;
        private Transform spawnParent;

        public PrefabPoolPool(int initialCount, Transform spawnParent = null)
        {
            this.initialCount = initialCount;
            this.spawnParent = spawnParent;
        }

        public TPrefab get(TPrefab prefab)
        {
            AutoMonobehaviourPool<TPrefab> pool = null;
            if (!pools.TryGetValue(prefab, out pool))
            {
                pool = pools[prefab] = new AutoMonobehaviourPool<TPrefab>(prefab, initialCount, spawnParent);
            }

            return pool.getItem();
        }
    }
}