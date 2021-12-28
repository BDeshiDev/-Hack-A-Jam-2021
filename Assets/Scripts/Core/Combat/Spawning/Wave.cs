using System;
using System.Collections.Generic;

namespace Core.Combat.Spawning
{
 
    [Serializable]
    public class Wave
    {
        
        public List<SpawnSlot> slots;
        public  int totalCount;
        public void startSpawn(Spawner spawner)
        {
            int runningSpawnCount = totalCount;
            spawner.remainingCountInWave += runningSpawnCount;
            foreach (var s in slots)
            {
                s.setCount(ref runningSpawnCount);
                s.startSpawn(spawner);

                if (runningSpawnCount == 0)
                    break;
            }
            spawner.remainingCountInWave -= runningSpawnCount;
        }
        
    }
    
}