using BDeshi.Utility.Extensions;
using UnityEngine;

namespace Core.Player
{
    public class HypnoAttacker : MonoBehaviour
    {
        [SerializeField] Transform shootIndicator;
        // public float indicatorDistance = 4;
        // public float indicatorAngleOffset = 0;
        
        public void updateIndicator(Vector3 aimPoint)
        {
            // if we just want dir we'll swap to this
            // shootIndicator.transform.localPosition = indicatorDistance * Vector3.up;
            // shootIndicator.transform.allignToDir(dir, indicatorAngleOffset);
            shootIndicator.position = aimPoint;
        }
    }
}
