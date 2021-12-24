using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Input;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Core.Combat
{
    
    public class Spawner: MonoBehaviourSingletonPersistent<Spawner>
    {
        public Transform arena;
        [SerializeField] Vector2 spawnPadding = 2f * Vector2.one;
        [SerializeField] Vector2 spawnRange;
        [SerializeField] private int waveIndex = 0;
        [SerializeField] private SummoningCircle summoningCirclePrefab;
        [SerializeField] private int actualWaveCount = 0;
        public List<Wave> waves;
        public event Action<int> waveCompleted;
        public int remainingCountInWave = 0;
    
        private HashSet<HypnoComponent> hypnotizedTracker = new HashSet<HypnoComponent>();

        [SerializeField] bool repeatLastWave = true;
        public void Start()
        {
            spawnNextWave();
        }

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

            public void startSpawn()
            {
                for (int i = 0; i < count; i++)
                {
                    var summoningCircle = PoolManager.Instance.summoningCircles.get(Spawner.Instance.summoningCirclePrefab);
                    Spawner.Instance.StartCoroutine(
                        summoningCircle.summon(Spawner.Instance.findSafeSpawnSpot(), prefab, spawned)
                    );
                }
            

            }
        }
        
        [Serializable]
        public class Wave
        {
            
            public List<SpawnSlot> slots;
            public  int totalCount;
            public void startSpawn()
            {
                int runningSpawnCount = totalCount;
                Spawner.Instance.remainingCountInWave += runningSpawnCount;
                foreach (var s in slots)
                {
                    s.setCount(ref runningSpawnCount);
                    s.startSpawn();

                    if (runningSpawnCount == 0)
                        break;
                }
                Spawner.Instance.remainingCountInWave -= runningSpawnCount;
            }
            
        }

        public Vector2 findSafeSpawnSpot()
        {
            Vector2 randomFactor = (Random.insideUnitCircle) * .5f;//0->1
            Vector3 point = new Vector2(
                spawnRange.x * (randomFactor.x),
                spawnRange.y * (randomFactor.y)
            );

            return arena.position + point;
        }

        protected override void initialize()
        {
            spawnRange = new Vector2(
                (arena.transform.localScale.x - spawnPadding.x),
                (arena.transform.localScale.y - spawnPadding.y)
                    );
        }

        void spawnNextWave()
        {
            if (waveIndex < waves.Count)
            {
                hypnotizedTracker.Clear();
                hypnos.Clear();

                actualWaveCount++;
                if (waveIndex == (waves.Count - 1) && repeatLastWave)
                {
                    waves[waveIndex].startSpawn();
                }
                else
                {                    

                    waves[waveIndex++].startSpawn();
                }

                Debug.Log($"spawning wave {waveIndex}(actually : {actualWaveCount}) started");

            }
            else
            {
                Debug.Log("all waves completed");
            }
        }

        public void handleSpawnedEnemyDeath(CombatEntity e)
        {
            //ugly but will do for noww
            Debug.Log("died " + e , e);

            var enemy = e as EnemyEntity;
            if (enemy != null)
            {
                unTrackEnemy(enemy);
            }

            remainingCountInWave--;
            if (remainingCountInWave == 0)
            {
                spawnNextWave();
            }
        }

        void unTrackEnemy(EnemyEntity e)
        {
            e.Died -= Spawner.Instance.handleSpawnedEnemyDeath;

            e.HypnoComponent.Hypnotized -= handleSpawnedEnemyHypnotized;
            e.HypnoComponent.HypnosisRecovered -= handleSpawnedEnemyDeHypnotized;
            e.HypnoComponent.Berserked -= handleSpawnedEnemyDeHypnotized;
        }
        
        public void trackEnemy(EnemyEntity e)
        {
            e.Died += handleSpawnedEnemyDeath;
            
            e.HypnoComponent.Hypnotized += handleSpawnedEnemyHypnotized;
            e.HypnoComponent.HypnosisRecovered += handleSpawnedEnemyDeHypnotized;
            e.HypnoComponent.Berserked += handleSpawnedEnemyDeHypnotized;
        }
# if UNITY_EDITOR
        private List<HypnoComponent> hypnos = new List<HypnoComponent>();
#endif
        //Assume a single enemy has a single hypnocomponent. Sufficient for the jam.
        public void handleSpawnedEnemyHypnotized(HypnoComponent hypnoComponent)
        {
            if (hypnotizedTracker.Add(hypnoComponent))
            {
                hypnos.Add(hypnoComponent);
                if (hypnotizedTracker.Count >= remainingCountInWave)
                {
                    Debug.Log("All enemeies hypno. Next stage!");
                    spawnNextWave();
                }
            }
        }
        
        public void handleSpawnedEnemyDeHypnotized(HypnoComponent hypnoComponent)
        {
            hypnotizedTracker.Remove(hypnoComponent);

            hypnos.Remove(hypnoComponent);
        }
    }
}