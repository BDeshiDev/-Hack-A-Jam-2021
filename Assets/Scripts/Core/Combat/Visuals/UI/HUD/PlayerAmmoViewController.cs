using BDeshi.UI;
using Core.Combat;
using Core.Combat.Enemies;
using UnityEngine;

namespace Core.UI
{
    public class PlayerAmmoViewController : MonoBehaviour
    {
        
        [SerializeField] private AmmoComponent ammoComponent;
        [SerializeField] private AmmoBar ammobar;
        public void init(AmmoComponent ammoComponent)
        {
            cleanUp();

            this.ammoComponent = ammoComponent;
            ammoComponent.onAmountChanged += OnAmountChanged;
            ammobar.init(ammoComponent);
            OnAmountChanged(ammoComponent);
        }
        
        
        
        private void OnAmountChanged(AmmoComponent ammoComponent)
        {
            ammobar.displayAmmo(ammoComponent.CurAmmo);
        }
        
        void OnDestroy()
        {
            cleanUp();
        }

        private void cleanUp()
        {
            if (this.ammoComponent != null)
            {
                this.ammoComponent.onAmountChanged -= OnAmountChanged;
            }
        }
    }
}