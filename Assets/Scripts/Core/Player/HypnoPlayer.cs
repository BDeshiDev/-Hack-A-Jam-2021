using System;
using Core.Physics;
using UnityEngine;

namespace Core.Player
{
    [RequireComponent(typeof(MoveComponent))]
    public class HypnoPlayer: MonoBehaviour
    {
        public MoveComponent mover { get; private set; }
        public HypnoAttacker Attacker { get; private set; }

        public void setLookDir(Vector3 dir, Vector3 aimPoint)
        {
            Attacker.updateAim(dir, aimPoint);
        }

        private void Awake()
        {
            mover = GetComponent<MoveComponent>();
            Attacker = GetComponent<HypnoAttacker>();
        }
        
        
    }
}