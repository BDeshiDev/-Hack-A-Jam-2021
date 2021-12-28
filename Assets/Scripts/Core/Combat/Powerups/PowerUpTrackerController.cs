using System.Collections.Generic;
using BDeshi.Utility;
using Core.Combat.Visuals.UI;
using UnityEngine;

namespace Core.Combat.Powerups
{
    public class PowerUpTrackerController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]private WorldPosTracker trackerPrefab;
        public Dictionary<Powerup, WorldPosTracker> trackers = new Dictionary<Powerup, WorldPosTracker>();
        private SimpleManualMonoBehaviourPool<WorldPosTracker> trackerPool;

        void Start()
        {
            trackerPool = new SimpleManualMonoBehaviourPool<WorldPosTracker>(trackerPrefab, 1);
        
            CombatEventManger.Instance.OnPowerUpSpawned.add(gameObject, handlePowerUpSpawned); 
            CombatEventManger.Instance.OnPowerUpDeSpawned.add(gameObject, handlePowerUpDespawned); 
        }

        private void handlePowerUpDespawned(Powerup p)
        {
            if (trackers.TryGetValue(p, out var tracker))
            {
                trackerPool.returnItem(tracker);
            }
        
        }

        private void handlePowerUpSpawned(Powerup p)
        {
            trackers[p] = trackerPool.getItem();
            trackers[p].init(p.Spriter);
        }


    }
}
