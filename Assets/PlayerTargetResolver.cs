using System.Collections;
using System.Collections.Generic;
using Core.Combat;
using Core.Input;
using UnityEngine;

public class PlayerTargetResolver : TargetResolverComponent
{
    public override Vector3 getTargetPos()
    {
        return InputManager.AimPoint;
    }
}
