using System;
using Core.Input;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Core.Player
{
    public class PlayerController:MonoBehaviour
    {
        private HypnoPlayer player;
        private void Awake()
        {
            player = GetComponent<HypnoPlayer>();
        }

        private void Start()
        {
            InputManager.Instance.AimOrigin = transform;
            
            InputManager.meleeButton.addPerformedCallback(gameObject, player.Attacker.handleMeleeHeld);
            InputManager.meleeButton.addCancelledCallback(gameObject, player.Attacker.handleMeleeReleased);
        }

        void Update()
        {
            player.mover.moveInputThisFrame = InputManager.NormalizedMoveInput;

            if (InputManager.IsAimActive)
            {
                player.setLookDir(InputManager.AimDir,InputManager.AimPoint);
            }
        }
        
        



    }
}