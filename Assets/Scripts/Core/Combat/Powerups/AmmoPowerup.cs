using Core.Player;

namespace Core.Combat.Powerups
{
    public class AmmoPowerup : Powerup
    {
        protected override void doPowerUpPickup(HypnoPlayer hypnoPlayer)
        {
            var ammoComponenet = hypnoPlayer.PlayerGunState.Gun.AmmoComponent;
            ammoComponenet.reload(ammoComponenet.MaxAmmo + ammoComponenet.MaxOverFlow);
        }
    }
}