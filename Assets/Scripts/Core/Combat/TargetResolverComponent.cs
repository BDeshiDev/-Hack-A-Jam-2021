using UnityEngine;

namespace Core.Combat
{
    /// <summary>
    /// Since enemies can get hypnotized and change sides on a whim
    /// Need a single place to get all of them
    /// And manage aggro.
    /// </summary>
    public class TargetResolverComponent: MonoBehaviour
    {
        public TargettingInfo targettingInfo;
        // public Vector3 targetPos;
    }
}