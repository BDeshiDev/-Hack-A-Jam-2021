using UnityEngine;

namespace Core.UI
{
    public class PlayerAmmoViewController : MonoBehaviour
    {
        //
        // [SerializeField] private ModularGun gun;
        // [SerializeField] private AmmoReloadComponent reloader;
        // [SerializeField] private AmmoBar ammobar;
        // [SerializeField] private AmmoBar extraAmmobar;
        // [SerializeField] private ProgressBar reloadBar;
        // public void init(ModularGun gun)
        // {
        //     this.gun = gun;
        //
        //     gun.AmmoComponent.onAmountChanged += OnAmountChanged;
        //     OnAmountChanged(gun.AmmoComponent);
        //
        //     reloader = gun.GetComponent<AmmoReloadComponent>();
        //     reloader.onReloadStarted += handleReloadStarted;
        //     reloader.onReloadCompleted += OnReloadCompleted;
        //
        //     reloadBar.init(reloader);
        // }
        //
        // private void OnReloadCompleted(AmmoReloadComponent obj)
        // {
        //     reloadBar.gameObject.SetActive(false);
        //     extraAmmobar.gameObject.SetActive(gun.AmmoComponent.IsOverflowing);
        // }
        //
        // private void handleReloadStarted(AmmoReloadComponent obj)
        // {
        //     reloadBar.gameObject.SetActive(true);
        //     extraAmmobar.gameObject.SetActive(false);
        // }
        //
        // public void cleanUp()
        // {
        //     if (gun != null)
        //     {
        //        gun.AmmoComponent.onAmountChanged -= OnAmountChanged;
        //     }
        //
        //     if (reloader != null)
        //     {
        //         reloader.onReloadStarted -= handleReloadStarted;
        //         reloader.onReloadCompleted -= OnReloadCompleted;
        //     }
        // }
        //
        // private void OnAmountChanged(AmmoComponent ammoComponent)
        // {
        //     ammoComponent.getAmmoCount(out var normalAmmo, out var overflowAmmo);
        //     ammobar.displayAmmo(normalAmmo);
        //     if (overflowAmmo > 0)
        //     {
        //         extraAmmobar.gameObject.SetActive(true);
        //         extraAmmobar.displayAmmo(overflowAmmo);
        //     }
        //     else
        //     {
        //         extraAmmobar.gameObject.SetActive(false);
        //     }
        // }
        //
        // void OnDestroy()
        // {
        //     cleanUp();
        // }
    }
}