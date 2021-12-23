using System;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using Core.Combat;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Player
{
    public class HypnoAttacker : MonoBehaviour
    {
        [SerializeField] Transform shootIndicator;
        // public float indicatorDistance = 4;
        public float rotationAngleOffset = -90;
        
        [SerializeField] private SpriteRenderer hypnoSlashSprite;
        [SerializeField] private HitBox hypnoSlashHitbox;
        [SerializeField] private bool isChargingMelee = false;
        [SerializeField] ChargableList<SimpleHitBoxAttack> meleeAttacks;
        //updating rotation of rigidbody directly has issues
        //moreover, there are parts of the sprite that we do not want to rotate
        [SerializeField] private Transform meleeHitboxContainer;
        public void updateAim(Vector3 dir, Vector3 aimPoint)
        {
            // if we just want dir we'll swap to this
            // shootIndicator.transform.localPosition = indicatorDistance * Vector3.up;
            // shootIndicator.transform.allignToDir(dir, indicatorAngleOffset);
            
            meleeHitboxContainer.allignToDir(dir, rotationAngleOffset);
            shootIndicator.position = aimPoint;
        }

        private void Awake()
        {
            shootIndicator.parent = null;
        }

        public void handleMeleeHeld()
        {
            isChargingMelee = true;
        }

        void Update()
        {
            if (isChargingMelee)
            {
                meleeAttacks.updateCharge(Time.deltaTime);
            }
        }

        public void handleMeleeReleased()
        {
            isChargingMelee = false;
            var attack = meleeAttacks.getItemAndReset();
            attack.startAttack();
        }
        
    }
}
