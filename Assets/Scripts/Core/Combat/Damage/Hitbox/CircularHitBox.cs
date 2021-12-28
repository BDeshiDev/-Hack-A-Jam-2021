using System;
using UnityEngine;

namespace Core.Combat
{
    public class CircularHitBox : HitBox
    {

        protected override Collider2D[] getOverlaps()
        {
            return Physics2D.OverlapCircleAll(transform.position,
                transform.lossyScale.x, 
                targetter.TargettingInfo.DamageMask);
        }
        
        protected override void drawGizmo()
        {
            Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x);
        }

    }
}