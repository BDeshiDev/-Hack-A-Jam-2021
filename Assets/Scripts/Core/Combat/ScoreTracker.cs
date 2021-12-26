using System;
using System.Collections.Generic;
using bdeshi.utility;

namespace Core.Combat
{
    public class ScoreTracker: MonoBehaviourLazySingleton<ScoreTracker>
    {
        public float survivalTime = 0;
        public float totalHypnoTime = 0;
        public int totalEnemyDeathCount = 0;
        public int totalHypnoChain = 0;
        
        public void addSurvivalTime(float t) => survivalTime += t;
        public void addHypnoTime(float t) => totalHypnoTime += t;
    }
}