using System;
using BDeshi.Utility;
using Core.Combat.Entities.Player;
using Core.Combat.Spawning;
using UnityEngine;

namespace Core.Combat.Shooting
{
    /// <summary>
    /// Auto reload over time.
    /// Only player needs to reloa for the jam
    /// Sufficient for the jam
    /// </summary>
    public class PlayerAmmoReloadComponent : MonoBehaviour
    {
        [SerializeField] private AmmoComponent ammoComponent;
        [SerializeField] private Gun gun;
        [SerializeField] private FiniteTimer ammoReloadTimer = new FiniteTimer(0, 3f);
        [SerializeField] private HypnoPlayer player;
        [SerializeField] int DashRecoverAmount = 2;
        [SerializeField] float hypnotizedCountBulletConversionFactor = .5f;
        public event Action<AmmoComponent> AmmoChanged;

        void Start()
        {
            ammoComponent = GetComponent<AmmoComponent>();
            gun = GetComponent<Gun>();
            gun.ShotFired.AddListener(resetReloadTimer);

            player = GetComponentInParent<HypnoPlayer>();
            
            CombatEventManger.Instance.OnSuccessFullDodge.add(gameObject, handleDodge);
            CombatEventManger.Instance.OnWaveCompleted.add(gameObject, handleWaveCompleted);
        }

        private void handleWaveCompleted(Spawner spawner)
        {
            int bonusAmmo = Mathf.CeilToInt(
                            spawner.NumEnemiesCurrentlyHypnotized
                             * hypnotizedCountBulletConversionFactor
                            );
            reload(bonusAmmo);
        }


        private void handleDodge()
        {
            reload(DashRecoverAmount);
        }


        private void resetReloadTimer()
        {
            ammoReloadTimer.reset();
        }
        

        private void reload(int count = 1)
        {
            ammoReloadTimer.reset();
            ammoComponent.reload(count);
        }
        

        void Update()
        {
            if(ammoComponent.IsFull)
                return;
            ammoReloadTimer.updateTimer(Time.deltaTime);
            if (ammoReloadTimer.isComplete)
            {
                reload();
            }
        }


    }
}
