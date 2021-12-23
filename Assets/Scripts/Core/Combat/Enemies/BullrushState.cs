using BDeshi.Utility;
using Core.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Combat.Enemies
{
    public class BullrushState: AttackState
    {
        public Vector2 bullrushDir;
        public FiniteTimer timer = new FiniteTimer(.3f);

        public override bool IsComplete => timer.isComplete;
        public float bullrushSpeedMultiplier = 2;
        public AnimationCurve bullrushSpeedCurve;
        public HitBox bullrushHitBox;

        public override void EnterState()
        {
            timer.reset();
            //could be used as  general dashs state if input and player refs are moved.
            //Not necessary at the moment
            bullrushDir = blobEntity.LastLookDir;
            bullrushHitBox.startDetection();
        }

        public override void Tick()
        {
            if(!timer.isComplete)
            {
                timer.updateTimer(Time.deltaTime);

                blobEntity.MoveComponent.moveInputThisFrame = bullrushDir 
                                                              * bullrushSpeedCurve.Evaluate(timer.Ratio) 
                                                              * bullrushSpeedMultiplier;
            }

        }

        public override void ExitState()
        {
            bullrushHitBox.stopDetection();
        }
    }
}