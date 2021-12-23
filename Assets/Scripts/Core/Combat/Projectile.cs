using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Combat
{
    public class Projectile : MonoBehaviour
    {
        public Vector3 sizeFactor = Vector3.one;
        public Vector3 CurSize => sizeFactor.multiplyDimensions(transform.localScale);
        [SerializeField] FiniteTimer durationTimer = new FiniteTimer(0,5);
        [SerializeField] private float speed = 5;
        [SerializeField] private UnityEvent onHit;
        [SerializeField] private float knockbackForce = 6;

        public TargettingInfo targetingInfo;
        public Vector3 ShotDir => transform.right;
        public DamageInfo damage;
        public void initialize(Vector3 spawnPos, Vector3 dir)
        {
            transform.position = spawnPos;
            transform.allignToDir(dir);

            durationTimer.reset();
        }

        void Update()
        {
            if (!durationTimer.isComplete)
            {
                durationTimer.updateTimer(Time.deltaTime);
                move(Time.deltaTime);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position,CurSize );
        }
        


        private RaycastHit2D queryCollision( float checkDistance)
        {
            return Physics2D.BoxCast(transform.position,
                CurSize,
                ShotDir.get2dAngle(),
                ShotDir, 
                checkDistance,
                targetingInfo.getCombinedLayerMask());
            
        }
        private void move(float delta)
        {
            var moveAmount = speed * delta;
            var hit = queryCollision(moveAmount);
            
            if (hit && hit.collider != null )
            {
                if (targetingInfo.DamageMask.Contains(hit))
                {
                    var d = hit.collider.GetComponent<IDamagable>();
                    d.takeDamage(damage);
                }

                handleHit();
                transform.position += ShotDir * hit.distance;
            }
            else
            {
                transform.position += ShotDir * moveAmount;
            }
        }

        protected void handleHit()
        {
            onHit.Invoke();
            Destroy(gameObject);
        }
        
    }
}