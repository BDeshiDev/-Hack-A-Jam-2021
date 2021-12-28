using Core.Input;

namespace Core.Combat.Enemies
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