﻿using BDeshi.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat
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
        public SingleUnityLayer PlayerLayer;
        /// <summary>
        /// Assume that this will be single layer and we can assign to go.layer directly
        /// </summary>
        public SingleUnityLayer EnemyLayer;
        public LayerMask getCombinedLayerMask()
        {
            return damageMask | obstacleMask;
        }
    }
}