using BDeshi.Utility;
using Core.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat.Enemies
{
    public class BullrushState: AttackState
    {
        public Vector2 bullrushDir;
        [FormerlySerializedAs("timer")] public FiniteTimer bullrushTimer = new FiniteTimer(.3f);
        [FormerlySerializedAs("timer")] public FiniteTimer postBullrushTimer = new FiniteTimer(1.87f);

        public override bool IsComplete => bullrushTimer.isComplete && postBullrushTimer.isComplete;
        public float bullrushSpeedMultiplier = 2;
        public AnimationCurve bullrushSpeedCurve;
        public HitBox bullrushHitBox;

        public override void EnterState()
        {
            bullrushTimer.reset();
            postBullrushTimer.reset();
            bullrushDir = blobEntity.LastLookDir;
            //could be used as  general dashs state if input and player refs are moved.
            //Not necessary at the moment
        }

        public override void Tick()
        {
            if(!bullrushTimer.isComplete)
            {   
                bullrushTimer.updateTimer(Time.deltaTime);

                blobEntity.MoveComponent.moveInputThisFrame = bullrushDir 
                                                              * bullrushSpeedCurve.Evaluate(bullrushTimer.Ratio) 
                                                              * bullrushSpeedMultiplier;
            }
            else
            {
                postBullrushTimer.safeUpdateTimer(Time.deltaTime);
            }

        }

        public override void ExitState()
        {

            bullrushHitBox.stopDetection();
        }
    }
}