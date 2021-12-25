using System;
using System.Collections.Generic;
using BDeshi.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Core.Combat.Enemies
{
    public class GunAttackState : AttackState
    {
        [FormerlySerializedAs("gun")] 
        [SerializeField] GunShot gunShot;
        public Transform shotPoint;
        
        [FormerlySerializedAs("postShotWaitTimer")] 
        [SerializeField] 
        FiniteTimer gunShotTimer = new FiniteTimer(1.8f);
        /// <summary>
        /// Loop through this during timer based on gunshotIndex, index++ when the time is matched and the shot isfired 
        /// </summary>
        [SerializeField] List<GunShotRound> gunshotRounds = new List<GunShotRound>();
        [SerializeField] private TargetResolverComponent targetResolverComponent;
        [SerializeField] private int gunShotIndex = 0;
        //Player can fire directly from enterstate
        [SerializeField] private bool shotFirstRoundAsap;
        //use for muzzle flashes
        [SerializeField] private UnityEvent ShotFired;

        [SerializeField] private bool allowMidAttackDirectionChange = false;
        [SerializeField] float maxAngleChangePerSec = 50;

        private void Awake()
        {
            targetResolverComponent = GetComponentInParent<TargetResolverComponent>();
        }

        public override void EnterState()
        {
            gunShotTimer.reset();
            gunShotIndex = 0;

            if (shotFirstRoundAsap && gunShotIndex < gunshotRounds.Count)
            {
                shootCurrentRound();
            }
        }

        public override void Tick()
        {
            gunShotTimer.safeUpdateTimer(Time.deltaTime);
            if (gunShotIndex < gunshotRounds.Count)
            {
                if (allowMidAttackDirectionChange)
                {
                    updateDirection();
                }

                if(gunshotRounds[gunShotIndex].shotTimeNormalized < gunShotTimer.Ratio)
                {
                    shootCurrentRound();
                }
            }
        }
        
        private void updateDirection()
        {
            Vector3 idealDir= (blobEntity.TargetResolverComponent.getTargetPos() - blobEntity.transform.position).normalized;

            
            Vector3 newLookDir = Vector3.RotateTowards(blobEntity.LastLookDir,
                                idealDir, 
                                maxAngleChangePerSec * Time.deltaTime,
                                1);//both already normalized, doesn't matter
            blobEntity.lookAlong(newLookDir);
        }

        void shootCurrentRound()
        {
            gunshotRounds[gunShotIndex++].gunShot.shoot(shotPoint,targetResolverComponent);
            
            ShotFired.Invoke();
        }

        public override void ExitState()
        {
            
        }

        public override bool IsComplete => gunShotTimer.isComplete;
    }
    
            
    [Serializable]
    public class GunShotRound
    {
        [Range(0,1)]
        public float shotTimeNormalized;
        public GunShot gunShot;
    }

}