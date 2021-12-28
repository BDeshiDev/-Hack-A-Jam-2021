using System;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public class Projectile : MonoBehaviour, AutoPoolable<Projectile>, IDamagable
    {
        public Vector3 sizeFactor = Vector3.one;
        public Vector3 CurSize => sizeFactor.multiplyDimensions(transform.localScale);
        [SerializeField] FiniteTimer durationTimer = new FiniteTimer(0, 5);
        [SerializeField] private float speed = 5;
        [SerializeField] private UnityEvent onHit;
        [SerializeField] private float knockbackForce = 6;
        [SerializeField] private SpriteRenderer spriter;
        [SerializeField]private ParticleHelper hitParticlesPrefab;

        public TargettingInfo TargetingInfo;
        public Vector3 ShotDir => transform.right;
        public DamageInfo damage;
        public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0,1,0,0);

        public void initialize(Vector3 spawnPos, Vector3 dir, TargettingInfo targetingInfo)
        {
            transform.position = spawnPos;
            transform.allignToDir(dir);
            //projectiles should not magically change damagelayers if enemy gets brainwashed
            //maybe lasers  but I'm not bothering with that now
            TargetingInfo = targetingInfo;
            durationTimer.reset();

            spriter.sortingLayerID = targetingInfo.ProjectileSortingLayerID;
            spriter.color = targetingInfo.ProjectileColorPrimary;
        }

        void Update()
        {
            if (!durationTimer.isComplete)
            {
                durationTimer.updateTimer(Time.deltaTime);
                move(Time.deltaTime);
            }
            else
            {
                handleEnd();
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, CurSize);
        }



        private RaycastHit2D queryCollision(float checkDistance)
        {
            return Physics2D.BoxCast(transform.position,
                CurSize,
                ShotDir.get2dAngle(),
                ShotDir,
                checkDistance,
                TargetingInfo.getCombinedLayerMask());

        }

        private void move(float delta)
        {
            var moveAmount = speedCurve.Evaluate(durationTimer.Ratio) *  speed * delta;
            var hit = queryCollision(moveAmount);

            if (hit && hit.collider != null)
            {
                if (TargetingInfo.DamageMask.Contains(hit))
                {
                    var d = hit.collider.GetComponent<IDamagable>();
                    d.takeDamage(damage);
                }
                // Debug.Log(gameObject + " hit " +hit.collider ,  hit.collider);
                handleHit(hit.point);
                transform.position += ShotDir * hit.distance;
            }
            else
            {
                transform.position += ShotDir * moveAmount;
            }
        }

        protected void handleHit(Vector2 point)
        {
            onHit.Invoke();

            var particles = GameplayPoolManager.Instance.particlePool
                .get(hitParticlesPrefab);
            particles.setColor(TargetingInfo.ProjectileColorPrimary);
            particles.transform.position = point;
            particles.transform.right = -transform.right;
            
            handleEnd();
        }

        public void handleEnd()
        {
            // Destroy(gameObject);
            NormalReturn();
        }

        public void initialize()
        {
            
        }

        public void handleForceReturn()
        {
            
        }
        
        public void NormalReturn()
        {
            handleForceReturn();
            NormalReturnCallback?.Invoke(this);
        }

        public event Action<Projectile> NormalReturnCallback;
        public void takeDamage(DamageInfo damage)
        {
            handleHit(transform.position);
        }
    }
}