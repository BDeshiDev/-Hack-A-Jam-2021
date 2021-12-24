using BDeshi.Utility;
using Core.Combat;
using Core.Input;
using UnityEngine;

namespace Core.Player
{
    public class PlayerChargableAttackState: PlayerState
    {
        public bool IsComplete => pickedAttack != null && pickedAttack.IsComplete;
        public float moveSpeedMult = .65f;

        public ChargableList<AttackState> chargedAttacks;
        private AttackState pickedAttack;
        public  override void EnterState()
        {
            pickedAttack = null;
        }

        public override void Tick()
        {
            if (pickedAttack == null)
            {
                chargedAttacks.updateCharge(Time.deltaTime);
                
                player.MoveComponent.moveInputThisFrame = InputManager.NormalizedMoveInput * moveSpeedMult;
                player.look(InputManager.AimDir, InputManager.AimPoint);
            }
            else
            {
                pickedAttack.Tick();
            }
        }

        public void handleChargeReleased()
        {
            if(pickedAttack!= null)
            {
                pickedAttack.ExitState();
            }
            
            pickedAttack = chargedAttacks.getItemAndReset();
            pickedAttack.EnterState();
        }

        public override void ExitState()
        {
            if(pickedAttack!= null)
                pickedAttack.ExitState();
            pickedAttack = null;
        }
    }
}