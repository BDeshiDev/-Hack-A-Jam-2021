using UnityEngine;

namespace Core.Combat.Targetting
{
    /// <summary>
    /// Abstraction over targets
    /// Since enemies can get hypnotized and change sides on a whim
    /// Need a single place to get target
    /// </summary>
    public abstract class TargetResolverComponent: MonoBehaviour
    {
        public TargettingInfo TargettingInfo => targettingInfo;
        [SerializeField] protected TargettingInfo targettingInfo;
        
        public abstract Vector3 getTargetPos();
    }
}