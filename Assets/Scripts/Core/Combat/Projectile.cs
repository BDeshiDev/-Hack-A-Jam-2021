using System;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.Combat
{
    public class Projectile : MonoBehaviour, AutoPoolable<Projectile>
    {
        public Vector3 sizeFactor = Vector3.one;
        public Vector3 CurSize => sizeFactor.multiplyDimensions(transform.localScale);
        [SerializeField] FiniteTimer durationTimer = new FiniteTimer(0, 5);
        [SerializeField] private float speed = 5;
        [SerializeField] private UnityEvent onHit;
        [SerializeField] private float knockbackForce = 6;

        public TargettingInfo TargetingInfo;
        public Vector3 ShotDir => transform.right;
        public DamageInfo damage;
        [SerializeField]private SpriteRenderer spriter;

        public void initialize(Vector3 spawnPos, Vector3 dir, TargettingInfo targetingInfo)
        {
            transform.position = spawnPos;
            transform.allignToDir(dir);
            //projectiles should not magically change damagelayers if enemy gets brainwashed
            //maybe lasers  but I'm not bothering with that now
            TargetingInfo = targetingInfo;
            durationTimer.reset();
            
            spriter.sortingLayerID = SortingLayer.NameToID(targetingInfo.projectileSortingLayer);
            spriter.color = targetingInfo.ProjectileColor;
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
            var moveAmount = speed * delta;
            var hit = queryCollision(moveAmount);

            if (hit && hit.collider != null)
            {
                if (TargetingInfo.DamageMask.Contains(hit))
                {
                    var d = hit.collider.GetComponent<IDamagable>();
                    d.takeDamage(damage);
                }
                // Debug.Log(gameObject + " hit " +hit.collider ,  hit.collider);
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
            handleEnd();
        }

        public void handleEnd()
        {
            // Destroy(gameObject);
            forceReturn();
        }

        public void initialize()
        {
            
        }

        public void forceReturn()
        {
            ReturnCallback?.Invoke(this);
        }

        public event Action<Projectile> ReturnCallback;
    }
}