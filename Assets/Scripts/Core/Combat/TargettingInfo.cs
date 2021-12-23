using UnityEngine;

namespace Core.Combat
{
    [CreateAssetMenu(fileName = "TargettingInfo")]
    public class TargettingInfo: ScriptableObject
    {
        [SerializeField]private LayerMask damageMask;
        public LayerMask DamageMask => damageMask;
        [SerializeField]private LayerMask obstacleMask;
        public LayerMask ObstacleMask => obstacleMask;
        public LayerMask getCombinedLayerMask()
        {
            return damageMask | obstacleMask;
        }
    }
}