using BDeshi.UI;
using Core.Combat;
using UnityEngine;

namespace Core.UI
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