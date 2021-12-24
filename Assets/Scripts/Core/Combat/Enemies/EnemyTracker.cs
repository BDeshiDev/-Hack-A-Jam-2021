using System.Collections.Generic;
using bdeshi.utility;
using BDeshi.Utility.Extensions;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Combat.Enemies
{
    public class EnemyTracker: MonoBehaviourLazySingleton<EnemyTracker>
    {
        [SerializeField] List<EnemyEntity> activeEnemies = new List<EnemyEntity>();

        public static void addNewActiveEnemy(EnemyEntity e)
        {
            Instance.activeEnemies.Add(e);
        }

        public static void removeInactiveEnemy(EnemyEntity e)
        {
            if(Instance == null)
                return;
            //this won't be called frequently, perf is irrelevant
            Instance.activeEnemies.Remove(e);
        }

        static int compare(EnemyEntity a, EnemyEntity b)
        {
            return a.HealthComponent.Cur.CompareTo(b.HealthComponent.Cur);
        }

        [CanBeNull]
        public static EnemyEntity getLowesHpNormalEnemy()
        {
            //health does not change quickly so this shouldn't be too bad
            Instance.activeEnemies.Sort(compare);

            foreach (var activeEnemy in Instance.activeEnemies)
            {
                if (!activeEnemy.IsHypnotized)
                    return activeEnemy;
            }

            return null;
        }
        
        [CanBeNull]
        public static EnemyEntity getRandomEnemy(EnemyEntity exclude)
        {
            //health does not change quickly so this shouldn't be too bad
            if(Instance.activeEnemies.Count <= 2)
                return null;
            return Instance.activeEnemies.getRandomItemExcluding(exclude);
        }
    }
}