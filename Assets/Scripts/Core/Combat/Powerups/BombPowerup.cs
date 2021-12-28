using Core.Combat.Entities.Player;

namespace Core.Combat.Powerups
{
    public class BombPowerup : Powerup
    {
        protected override void doPowerUpPickup(HypnoPlayer hypnoPlayer)
        {
            hypnoPlayer.GetComponent<PlayerBombLauncher>().addBomb();
        }
    }
}