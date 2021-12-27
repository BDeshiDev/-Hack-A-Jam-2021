using System;
using BDeshi.Utility;
using Core.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat.Powerups
{
    public abstract class Powerup: MonoBehaviour, AutoPoolable<Powerup>
    {
        public FiniteTimer durationTimer = new FiniteTimer(9);
        public UnityEvent OnPowerUpSpawned;
        public UnityEvent OnPowerUpPickedUp;
        [SerializeField] private SpriteRenderer spriter;
        public SpriteRenderer Spriter => spriter;
        void Update()
        {
            if (durationTimer.tryCompleteTimer(Time.deltaTime))
            {
                NormalReturn();
            }
        }
    

        private void NormalReturn()
        {
            CombatEventManger.Instance.OnPowerUpDeSpawned.Invoke(this);
            NormalReturnCallback?.Invoke(this);
        }

        public void initialize()
        {
            durationTimer.reset();
            OnPowerUpSpawned.Invoke();
            CombatEventManger.Instance.OnPowerUpSpawned.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var p = other.GetComponent<HypnoPlayer>();
            if (p != null)
            {
                doPowerUpPickup(p);
                OnPowerUpPickedUp.Invoke();
                NormalReturn();
            }
        }

        protected abstract void doPowerUpPickup(HypnoPlayer hypnoPlayer);


        public void handleForceReturn()
        {
            
        }

        public event Action<Powerup> NormalReturnCallback;
    }
}