using System.Collections.Generic;
using BDeshi.Utility.Extensions;
using UnityEngine;

namespace Core.Combat
{
    public class HitBox: MonoBehaviour
    {
        HitBoxStatus status;
        public LayerMask damageMask;
        public DamageInfo damagePerHit;
        private HashSet<IDamagable> damaged = new HashSet<IDamagable>();
        public bool showIfInactive = false;
        public void startDetection()
        {
            status = HitBoxStatus.Active;
            damaged.Clear();
        }
        
        public void stopDetection()
        {
            status = HitBoxStatus.Inactive;
            damaged.Clear();
        }

        private void Update()
        {
            if(status != HitBoxStatus.Inactive)
            {
                var results = Physics2D.OverlapBoxAll(transform.position,
                    transform.lossyScale,
                    transform.get2dAngle(),
                    damageMask);
                foreach (var result in results)
                {
                    var d = result.GetComponent<IDamagable>();
                    if (d != null && damaged.Add(d))
                    {
                        d.takeDamage(damagePerHit);
                    }
                }
            }
        }
        
        
        private void OnDrawGizmosSelected()
        {
            if(status == HitBoxStatus.Inactive && !showIfInactive)
                return;
            Gizmos.color = status == HitBoxStatus.Active
                ? Color.green
                : status == HitBoxStatus.Damaging
                    ? Color.red
                    : Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
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