using Core.Combat.Powerups;
using Core.Player;
using UnityEngine;

public class HealthPowerup : Powerup
{
    [SerializeField] float healPercentage = .4f;
    protected override void doPowerUpPickup(HypnoPlayer hypnoPlayer)
    {
        hypnoPlayer.HealthComponent.modifyAmount(hypnoPlayer.HealthComponent.Max * healPercentage);
    }
}