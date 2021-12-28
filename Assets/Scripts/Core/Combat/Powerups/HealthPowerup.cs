using Core.Combat.Entities.Player;
using UnityEngine;

namespace Core.Combat.Powerups
{
    public class HealthPowerup : Powerup
    {
        [SerializeField] float healPercentage = .4f;
        protected override void doPowerUpPickup(HypnoPlayer hypnoPlayer)
        {
            hypnoPlayer.HealthComponent.modifyAmount(hypnoPlayer.HealthComponent.Max * healPercentage);
        }
    }
}