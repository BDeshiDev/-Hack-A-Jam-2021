using System.Collections.Generic;
using Core.Combat.Damage;
using Core.Combat.Damage.Hitbox;
using Core.Combat.Targetting;
using UnityEngine;

namespace Core.Combat.Hitbox
{
    public abstract class HitBox : MonoBehaviour
    {
        [SerializeField] protected HitBoxStatus status;
        public TargetResolverComponent targetter;
        public DamageInfo damagePerHit;
        private HashSet<IDamagable> damaged = new HashSet<IDamagable>();
        public bool showIfInactive = false;

        protected abstract void drawGizmo();
        protected abstract Collider2D[] getOverlaps();
        
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
                var results = getOverlaps();
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
            drawGizmo();
        }

    }
}