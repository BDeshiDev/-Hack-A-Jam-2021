using System;
using BDeshi.Utility;
using Core.Combat.Enemies;
using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Auto reload over time. Sufficient for the jam
    /// </summary>
    public class AmmoReloadComponent : MonoBehaviour
    {
        [SerializeField] private AmmoComponent ammoComponent;
        [SerializeField] private Gun gun;
        [SerializeField] private FiniteTimer ammoReloadTimer = new FiniteTimer(0, 3f);
        
        public event Action<AmmoComponent> AmmoChanged;

        void Start()
        {
            ammoComponent = GetComponent<AmmoComponent>();
            gun = GetComponent<Gun>();
            gun.ShotFired.AddListener(resetReloadTimer);
            
            
        }
        

        private void resetReloadTimer()
        {
            ammoReloadTimer.reset();
        }
        

        private void reload()
        {
            ammoReloadTimer.reset();
            ammoComponent.reload(1);
        }
        

        void Update()
        {
            ammoReloadTimer.updateTimer(Time.deltaTime);
            if (ammoReloadTimer.isComplete)
            {
                reload();
            }
        }


    }
}
