using System;
using UnityEngine;

namespace Core.Combat.Entities.Player
{
    public class PlayerBombLauncher : MonoBehaviour
    {
        [SerializeField] PlayerBomb bomb;
        public event Action<bool> BombChargeToggled;
        public bool HasBomb { get; private set; } = true;

        public void handleBombLaunchAttempt()
        {
            if (HasBomb)
            {
                HasBomb = false;
            
                bomb.launchBomb(transform.position);
                BombChargeToggled?.Invoke(false);
            }
        }


        public void forceRemoveBOmb()
        {
            HasBomb = false;
        }

        public void addBomb()
        {
            HasBomb = true;
            BombChargeToggled?.Invoke(true);
        }
    }
}
