using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillBar : MonoBehaviour
{
    [SerializeField] Image image;
    
        
    public virtual void updateFromRatio(float ratio)
    {
        image.fillAmount = ratio;
    }

}
