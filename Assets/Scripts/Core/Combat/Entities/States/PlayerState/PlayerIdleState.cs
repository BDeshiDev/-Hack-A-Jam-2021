using Core.Input;

namespace Core.Combat.Entities.States.PlayerState
{
    public class PlayerIdleState: PlayerState
    {
        public override void EnterState()
        {
            
        }

        public override void Tick()
        {
            player.MoveComponent.moveInputThisFrame = InputManager.NormalizedMoveInput;
            
            if (InputManager.IsAimActive)
            {
                player.look(InputManager.AimDir,InputManager.AimPoint);
            }
        }

        public override void ExitState()
        {
            
        }
    }
}