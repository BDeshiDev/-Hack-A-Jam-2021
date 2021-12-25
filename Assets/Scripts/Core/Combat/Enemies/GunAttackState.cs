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
        [FormerlySerializedAs("postShotWaitTimer")] 
        [SerializeField] 
        FiniteTimer gunShotTimer = new FiniteTimer(1.8f);
        /// <summary>
        /// Loop through this during timer based on gunshotIndex, index++ when the time is matched and the shot is fired 
        /// </summary>
        [SerializeField] List<GunShotRound> gunshotRounds = new List<GunShotRound>();
        
        [SerializeField] private int gunShotIndex = 0;
        //Player can fire directly from enterstate
        [SerializeField] private bool shotFirstRoundAsap;
        //use for muzzle flashes
        [SerializeField] private bool allowMidAttackDirectionChange = false;
        [SerializeField] float maxAngleChangePerSec = 50;

        [SerializeField] private Gun gun;

        public override void EnterState()
        {
            gunShotTimer.reset();
            gunShotIndex = 0;

            if (shotFirstRoundAsap && gunShotIndex < gunshotRounds.Count)
            {
                shootCurrentRound();
            }
        }

        private void shootCurrentRound()
        {
            gun.shoot( gunshotRounds[gunShotIndex++].gunShot,blobEntity.TargetResolverComponent );
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

                if (gunShotIndex < gunshotRounds.Count
                    && gunshotRounds[gunShotIndex].shotTimeNormalized < gunShotTimer.Ratio
                )
                {
                    shootCurrentRound();
                }
            }
        }

        private Vector2 v;
        private void updateDirection()
        {
            Vector3 idealDir= (blobEntity.TargetResolverComponent.getTargetPos() - blobEntity.transform.position).normalized;

            
            Vector3 newLookDir = Vector3.RotateTowards(blobEntity.LastLookDir,
                                idealDir, 
                                maxAngleChangePerSec * Time.deltaTime,
                                1);//both already normalized, doesn't matter
            
            blobEntity.lookAlong(newLookDir);
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