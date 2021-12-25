using System;
using BDeshi.UI;
using Core.Combat;
using UnityEngine;

namespace Core.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private HealthComponent healthComponent;
        // [SerializeField] private ModularGun gun;

        [SerializeField] private PlayerAmmoViewController ammoViewController;
        [SerializeField] private PlayerHealthViewController healthViewController;
        [SerializeField] private PlayerBombLauncher bomber;
        [SerializeField] private ImageFillBar bombRechargeBar;
        private void OnEnable()
        {
            GameStateManager.Instance.GameplaySceneRefresh += refreshHUD;
        }
        
        private void OnDisable()
        {
            GameStateManager.Instance.GameplaySceneRefresh -= refreshHUD;
        }

        private void refreshHUD()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            healthComponent = p.GetComponent<HealthComponent>();
            healthViewController.init(healthComponent);

            bomber = p.GetComponent<PlayerBombLauncher>();
            //not necessary to unsub for jam since this will get destroyed 
            bomber.BombChargeUpdated += bombRechargeBar.updateFromRatio;
            bombRechargeBar.updateFromRatio(bomber.bombRechargeTimer.Ratio);
            AmmoComponent ammoComponent = p.GetComponentInChildren<AmmoComponent>();
            ammoViewController.init(ammoComponent);
        }

        void Start()
        {
            refreshHUD();
        }


    }
}
