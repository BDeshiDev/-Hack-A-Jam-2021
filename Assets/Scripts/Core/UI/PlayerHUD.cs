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
        void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            healthComponent = p.GetComponent<HealthComponent>();
            // gun = p.gun.GetComponent<ModularGun>();

            // ammoViewController.init(gun);

            healthViewController.init(healthComponent);
        }


    }
}
