using System;
using System.Collections.Generic;
using BDeshi.Utility;
using Core.Input;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Core.Combat
{
    
    public partial class Spawner: MonoBehaviour
    {
        public Transform arena;
        [SerializeField] Vector2 spawnPadding = 2f * Vector2.one;
        [SerializeField] Vector2 spawnRange;
        [SerializeField] private int waveIndex = 0;
        [SerializeField] private int actualWaveCount = 0;
        public SummoningCircle summoningCirclePrefab;

        public int lastReachedWave => actualWaveCount;
        
        public List<Wave> waves;
        public event Action<int> waveCompleted;
        public int remainingCountInWave = 0;
    
        private HashSet<HypnoComponent> hypnotizedTracker = new HashSet<HypnoComponent>();

        [SerializeField] bool repeatLastWave = true;
        public void Start()
        {
            spawnNextWave();
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

        private void Awake()
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