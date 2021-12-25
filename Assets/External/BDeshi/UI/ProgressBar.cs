using BDeshi.Utility;
using Core.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace BDeshi.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] Slider slider;
        private ResourceComponent target;
     

        private void OnDestroy()
        {
            cleanup();
        }

        public virtual void updateFromRatio(ResourceComponent resourceComponent)
        {
            slider.value = resourceComponent.Ratio;
        }
        
        public virtual void updateFromRatio(float ratio)
        {
            slider.value = ratio;
        }
        
        
        public void init(ResourceComponent r)
        {
            if(target != null){
                target.RatioChanged -= updateFromRatio;
            }

            target = r;
            target.RatioChanged += updateFromRatio;
            updateFromRatio(r);
        }

   

        public void cleanup() {
            if (target != null)
            {
                target.RatioChanged -= updateFromRatio;
            }
        }
    }
}
