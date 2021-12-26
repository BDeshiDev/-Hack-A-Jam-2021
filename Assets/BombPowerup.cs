using Core.Combat.Powerups;
using Core.Player;

public class BombPowerup : Powerup
{
    protected override void doPowerUpThing(HypnoPlayer hypnoPlayer)
    {
        hypnoPlayer.GetComponent<PlayerBombLauncher>().addBomb();
    }
}