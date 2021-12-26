using System;
using System.Collections;
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
    
        private HashSet<EnemyEntity> hypnotizedTracker = new HashSet<EnemyEntity>();
        //any enemy spawned and not dead go here
        private HashSet<EnemyEntity> spawnTracker = new HashSet<EnemyEntity>();

        [SerializeField] bool repeatLastWave = true;

        public float spawnerRunningTime { get; private set; } = 0;
        public float totalHypnoTime { get; private set; } = 0;
        public float totalEnemiesKilled { get; private set; } = 0;

        public IEnumerator Start()
        {
            CombatEventManger.Instance.OnEnemyDefeated.add(gameObject,handleSpawnedEnemyDeath);
            CombatEventManger.Instance.OnEnemyHypnotized.add(gameObject,handleSpawnedEnemyHypnotized);
            CombatEventManger.Instance.OnEnemyHypnosisRecovery.add(gameObject,handleSpawnedEnemyDeHypnotized);
            CombatEventManger.Instance.OnEnemyBerserked.add(gameObject,handleSpawnedEnemyHypnotized);

            yield return null;
            yield return null;
            yield return null;
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

        public void handleSpawnedEnemyDeath(EnemyEntity e)
        {
            //ugly but will do for noww
            if(spawnTracker.Remove(e))
            {
                hypnotizedTracker.Remove(e);
                spawnTracker.Remove(e);


                totalHypnoTime += e.TimeSpentHypnotized;


                totalEnemiesKilled++;

                remainingCountInWave--;
                if (remainingCountInWave == 0)
                {
                    spawnNextWave();
                }
            }
        }



        public void handleSpawnedEnemyHypnotized(EnemyEntity enemy)
        {
            if (spawnTracker.Contains(enemy) && hypnotizedTracker.Add(enemy))
            {

                if (hypnotizedTracker.Count >= remainingCountInWave)
                {
                    Debug.Log("All enemeies hypno. Next stage!");
                    spawnNextWave();
                }
            }
        }
        
        public void handleSpawnedEnemyDeHypnotized(EnemyEntity enemy)
        {
            if(spawnTracker.Contains(enemy))
                hypnotizedTracker.Remove(enemy);
        }

        private void Update()
        {
            spawnerRunningTime += Time.deltaTime;
        }


        public void trackEnemy(EnemyEntity e)
        {
            spawnTracker.Add(e);
        }
    }
}