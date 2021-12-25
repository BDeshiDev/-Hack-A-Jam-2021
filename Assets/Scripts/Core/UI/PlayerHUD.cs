using System;
using Core.Combat;
using UnityEngine;

namespace Core.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private HealthComponent healthComponent;
        // [SerializeField] private ModularGun gun;

        // [SerializeField] private PlayerAmmoViewController ammoViewController;
        [SerializeField] private PlayerHealthViewController healthViewController;

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
            Debug.Log("HUD target " + p , p);
            healthComponent = p.GetComponent<HealthComponent>();
            // gun = p.gun.GetComponent<ModularGun>();

            // ammoViewController.init(gun);

            healthViewController.init(healthComponent);
        }

        void Start()
        {
            refreshHUD();
        }


    }
}
