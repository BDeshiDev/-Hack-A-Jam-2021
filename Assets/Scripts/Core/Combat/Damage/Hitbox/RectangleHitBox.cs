using System.Collections.Generic;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public class RectangleHitBox: HitBox
    {

        protected override Collider2D[] getOverlaps()
        {
            return Physics2D.OverlapBoxAll(transform.position,
                transform.lossyScale,
                transform.get2dAngle(),
                targetter.TargettingInfo.DamageMask);
        }



        protected override void drawGizmo()
        {
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        }



    }
    public enum  HitBoxStatus
    {
        Inactive,
        Active,
        Damaging,
    }
}