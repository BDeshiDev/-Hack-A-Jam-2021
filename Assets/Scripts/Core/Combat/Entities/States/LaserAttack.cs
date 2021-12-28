using BDeshi.Utility;
using Core.Combat.Damage;
using Core.Combat.Targetting;
using UnityEngine;

namespace Core.Combat.Entities.States
{
    public class LaserAttack : AttackState
    {
        public Vector2 direction => transform.right;
        public TargetResolverComponent targetter;

        [SerializeField] float laserLen = 55;
        [SerializeField] LineRenderer liner;
        [SerializeField] private DamageInfo damagePerHit;
        [SerializeField] private FiniteTimer laserDuration = new FiniteTimer(.3f);
        [SerializeField] private FiniteTimer laserShotWaitDuration = new FiniteTimer(.4f);
        [SerializeField] Color baseColor = Color.red;
        
        
        void Awake()
        {
            if(liner == null)
                liner = GetComponent<LineRenderer>();
            liner.positionCount = 2;
            liner.useWorldSpace = true;
        }

        

        private void handleHit(RaycastHit2D hit, bool shouldDamage)
        {
            if (hit)
            {
                if(shouldDamage)
                {
                    if (hit.collider != null)
                    {
                        var d = hit.collider.GetComponent<IDamagable>();
                        if (d != null)
                        {
                            d.takeDamage(damagePerHit);
                        }
                    }
                }

                liner.SetPosition(1, (Vector3)hit.point);
            }
            else
            {
                liner.SetPosition(1, liner.transform.position + (Vector3)direction * laserLen);
            }
        }

        public override void EnterState()
        {
            laserDuration.reset();
            showLaser(false);
        }

        public void showLaser()
        {
            showLaser(false);
        }

        public void showLaser(bool shouldDamage)
        {
            liner.enabled = true;
            liner.sortingLayerID = targetter.TargettingInfo.ProjectileSortingLayerID;
            baseColor = shouldDamage
                ? targetter.TargettingInfo.ProjectileColorPrimary
                : targetter.TargettingInfo.ProjectileColorSecondary;
            setColor(baseColor);
            
            updateLaserEndpoints(shouldDamage);
        }

        public void updateLaserEndpoints(bool shouldDamage)
        {
            
            var hit = Physics2D.Raycast(
                liner.transform.position,
                direction, laserLen,
                targetter.TargettingInfo.getCombinedLayerMask());
            
            liner.SetPosition(0, liner.transform.position);
            handleHit(hit, shouldDamage);
        }



        public override void Tick()
        {
            if (laserShotWaitDuration.isComplete)
            {
                laserDuration.safeUpdateTimer(Time.deltaTime);
            
                Color endColor = baseColor;
                endColor.a = 0;
                setColor(Color.Lerp(baseColor, endColor, laserDuration.Ratio));
            }
            else
            {
                if (laserShotWaitDuration.tryCompleteTimer(Time.deltaTime))
                {
                    showLaser(true);
                }
                else
                {
                    showLaser(false);
                }
            }


            
            // prevent mutihiy before adding this
            // updateLaserEndpoints(true);
        }

        public void setColor(Color c)
        {
            liner.startColor = liner.endColor = c;

        }

        public override void ExitState()
        {
            hideLaser();
        }

        public void hideLaser()
        {
            liner.enabled = false;
        }

        public override bool IsComplete => laserDuration.isComplete;
    }
}
