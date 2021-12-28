using Core.Combat.CombatResources;
using Core.Combat.Entities.Player;
using Core.Combat.Shooting;
using Core.GameState;
using UnityEngine;

namespace Core.Combat.Visuals.UI.HUD
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private HealthComponent healthComponent;
        // [SerializeField] private ModularGun gun;

        [SerializeField] private PlayerAmmoViewController ammoViewController;
        [SerializeField] private PlayerHealthViewController healthViewController;
        [SerializeField] private PlayerBombLauncher bomber;
        
        //should be moved to a bomb view
        [SerializeField] private ImageFillBar bombRechargeBar;
        [SerializeField] private CanvasGroup bombCanvas;

        private void refreshHUD()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            healthComponent = p.GetComponent<HealthComponent>();
            healthViewController.init(healthComponent);

            AmmoComponent ammoComponent = p.GetComponentInChildren<AmmoComponent>();
            ammoViewController.init(ammoComponent);
            
            bomber = p.GetComponent<PlayerBombLauncher>();
            //not necessary to unsub for jam since this will get destroyed 
            bomber.BombChargeToggled += updateBombView;
            updateBombView(bomber.HasBomb);
        }

        private void updateBombView(bool available)
        {
            bombCanvas.alpha = available ? 1 : .5f;
        }

        void Start()
        {
            GameStateManager.Instance.GameplaySceneRefresh += refreshHUD;
            refreshHUD();
        }
        private void OnDestroy()
        {
            GameStateManager.Instance.GameplaySceneRefresh -= refreshHUD;
        }

    }
}
