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

        public virtual void updateFromRatio(ResourceComponent r)
        {
            slider.value = r.Ratio;
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
