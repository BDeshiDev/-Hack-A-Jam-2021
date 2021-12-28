using BDeshi.UI;
using Core.Combat.CombatResources;
using UnityEngine;

namespace Core.Combat.Visuals.UI.HUD
{
    public class PlayerHealthViewController : MonoBehaviour
    {
        public ProgressBar healthBar;
        public HealthComponent healthComponent;

        public void init(HealthComponent healthComponent)
        {
            this.healthComponent = healthComponent;

            healthBar.init(healthComponent);
        }
    }
}