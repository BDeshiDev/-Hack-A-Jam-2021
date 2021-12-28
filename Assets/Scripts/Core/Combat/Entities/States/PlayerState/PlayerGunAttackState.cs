using Core.Input;

namespace Core.Combat.Entities.States.PlayerState
{
    public class PlayerGunAttackState : GunAttackState
    {
        public override void Tick()
        {
            base.Tick();
            blobEntity.MoveComponent.moveInputThisFrame = InputManager.NormalizedMoveInput;
        }
    }
}