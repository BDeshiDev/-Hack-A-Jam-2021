using BDeshi.Utility;
using Core.Input;
using UnityEngine;

namespace Core.Player
{
    public class PlayerDashState: PlayerState
    {
        public Vector2 dashDir;
        public FiniteTimer dashTimer = new FiniteTimer();
        public Vector2 invulnWindow = new Vector2();

        public bool IsComplete => dashTimer.isComplete;
        public float dashSpeedMultiplier = 2;
        public AnimationCurve dashSpeedCurve;
        public override void EnterState()
        {
            dashTimer.reset();
            //could be used as  general dashs state if input and player refs are moved.
            //Not necessary at the moment
            dashDir = InputManager.NormalizedMoveInput;
        }

        public override void Tick()
        {
            if(!dashTimer.isComplete)
            {
                dashTimer.updateTimer(Time.deltaTime);

                player.MoveComponent.moveInputThisFrame = 
                    dashDir 
                    * dashSpeedCurve.Evaluate(dashTimer.Ratio) 
                    * dashSpeedMultiplier;
            }

            player.isInvulnerable = dashTimer.Ratio >= invulnWindow.x && dashTimer.Ratio <= invulnWindow.y;
        }

        public override void ExitState()
        {
            player.isInvulnerable = false;
        }
    }
}