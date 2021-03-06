using BDeshi.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat.Targetting
{
    [CreateAssetMenu(fileName = "TargettingInfo")]
    public class TargettingInfo: ScriptableObject
    {
        [SerializeField]private LayerMask damageMask;
        public LayerMask DamageMask => damageMask;
        [SerializeField]private LayerMask obstacleMask;
        public LayerMask ObstacleMask => obstacleMask;
        /// <summary>
        /// Assume that this will be single layer and we can assign to go.layer directly
        /// </summary>
        [FormerlySerializedAs("PlayerLayer")] public SingleUnityLayer HypnotizedLayer;
        /// <summary>
        /// Assume that this will be single layer and we can assign to go.layer directly
        /// </summary>
        [FormerlySerializedAs("EnemyLayer")] public SingleUnityLayer NormalLayer;
        [SerializeField] string projectileSortingLayer;
        
        public int ProjectileSortingLayerID =>SortingLayer.NameToID(projectileSortingLayer);
        [FormerlySerializedAs("ProjectileColor")] 
        public Color ProjectileColorPrimary = Color.cyan;
        public Color ProjectileColorSecondary = Color.cyan;

        public LayerMask getCombinedLayerMask()
        {
            return damageMask | obstacleMask;
        }
    }
}