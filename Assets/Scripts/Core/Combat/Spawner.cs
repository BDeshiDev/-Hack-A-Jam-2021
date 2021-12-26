using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Input;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Combat
{
    public class Spawner: MonoBehaviour
    {
        public Arena arena;

        [SerializeField] private int waveIndex = 0;
        [SerializeField] private int actualWaveCount = 0;
        public SummoningCircle summoningCirclePrefab;

        public int lastReachedWave => actualWaveCount;
        
        public List<Wave> waves;
        public event Action<int> waveCompleted;
        public int remainingCountInWave = 0;
    
        private HashSet<HypnoComponent> hypnotizedTracker = new HashSet<HypnoComponent>();

        [SerializeField] bool repeatLastWave = true;

        public float spawnerRunningTime { get; private set; } = 0;
        public float totalHypnoTime { get; private set; } = 0;
        public void Start()
        {
            spawnNextWave();
        }

        public Vector2 findSafeSpawnSpot()
        {
            return arena.findSafeSpawnSpot();
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
                    waves[waveIndex].startSpawn(this);
                }
                else
                {                    

                    waves[waveIndex++].startSpawn(this);
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

                totalHypnoTime += enemy.TimeSpentHypnotized;
            }

            remainingCountInWave--;
            if (remainingCountInWave == 0)
            {
                spawnNextWave();
            }
        }

        void unTrackEnemy(EnemyEntity e)
        {
            e.Died -= handleSpawnedEnemyDeath;

            e.HypnoComponent.Hypnotized -= handleSpawnedEnemyHypnotized;
            e.HypnoComponent.HypnosisRecovered -= handleSpawnedEnemyDeHypnotized;
            e.HypnoComponent.Berserked -= handleSpawnedEnemyHypnotized;
        }
        
        public void trackEnemy(EnemyEntity e)
        {
            e.Died += handleSpawnedEnemyDeath;
            
            e.HypnoComponent.Hypnotized += handleSpawnedEnemyHypnotized;
            e.HypnoComponent.HypnosisRecovered += handleSpawnedEnemyDeHypnotized;
            e.HypnoComponent.Berserked += handleSpawnedEnemyHypnotized;
        }
# if UNITY_EDITOR
        private List<HypnoComponent> hypnos = new List<HypnoComponent>();
#endif

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

        private void Update()
        {
            spawnerRunningTime += Time.deltaTime;
        }
    }
}