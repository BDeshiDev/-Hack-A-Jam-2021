using Core.Input;
using UnityEngine;

namespace Core.Combat.Targetting
{
    public class PlayerTargetResolver : TargetResolverComponent
    {
        public override Vector3 getTargetPos()
        {
            return InputManager.AimPoint;
        }
    }
}
