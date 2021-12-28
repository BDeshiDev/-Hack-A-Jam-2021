using UnityEngine;
using UnityEngine.UI;

namespace Core.Combat.Visuals.UI
{
    public class ImageFillBar : MonoBehaviour
    {
        [SerializeField] Image image;
    
        
        public virtual void updateFromRatio(float ratio)
        {
            image.fillAmount = ratio;
        }

    }
}
