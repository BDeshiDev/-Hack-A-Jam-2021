using System.Collections.Generic;
using bdeshi.utility;
using BDeshi.Utility;
using Core.Combat.Entities.Enemies;
using Core.Combat.Pooling.Extensions;
using Core.Combat.Powerups;
using Core.Combat.Shooting;
using Core.Combat.Spawning;
using Core.GameState;
using Core.Misc;
using UnityEngine;

namespace Core.Combat.Pooling
{
    /// <summary>
    /// Single place for pools. 
    /// Will automatically return objects on level change.
    /// Not optimal but sufficient for jam
    /// </summary>
    public class GameplayPoolManager: MonoBehaviourLazySingleton<GameplayPoolManager>
    {
        public PrefabPoolPool<Projectile> projectilePool;
        public PrefabPoolPool<EnemyEntity> enemyPool;
        public PrefabPoolPool<SummoningCircle> summoningCircles;
        public PrefabPoolPool<Powerup> powerUpPool;
        public PrefabPoolPool<ParticleHelper> particlePool;
        public PrefabPoolPool<PoolableSFXPlayer> sfxPool;

        protected override void initialize()
        {
            base.initialize();
            
            projectilePool = new PrefabPoolPool<Projectile>(20, createParent("Projectiles"));
            enemyPool = new PrefabPoolPool<EnemyEntity>(3, createParent("Enemies"));
            summoningCircles = new PrefabPoolPool<SummoningCircle>(1, createParent("summoningCircles"));
            powerUpPool = new PrefabPoolPool<Powerup>(1, createParent("Powerups"));
            particlePool = new PrefabPoolPool<ParticleHelper>(1, createParent("particles"));
            sfxPool = new PrefabPoolPool<PoolableSFXPlayer>(1, createParent("sfx"));
        }

        private void Start()
        {
            GameStateManager.Instance.GameplaySceneRefresh += handleLevelChange;
        }
        private void OnDestroy()
        {
            if(GameStateManager.Instance != null)
                GameStateManager.Instance.GameplaySceneRefresh -= handleLevelChange;
        }
        private void handleLevelChange()
        {
            projectilePool.forceReturnAll();
            enemyPool.forceReturnAll();
            summoningCircles.forceReturnAll();
        }

        public Transform createParent(string name)
        {
            var go = new GameObject(name).transform;
            go.SetParent(transform);

            return go.transform;
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

        public void forceReturnAll()
        {
            foreach (var pool in pools)
            {
                pool.Value.returnAll();
            }
        }

    }
}