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
            
            InputManager.meleeButton.addPerformedCallback(gameObject, player.handleMeleeHeld);
            InputManager.meleeButton.addCancelledCallback(gameObject, player.handleMeleeReleased);
            InputManager.bombButton.addPerformedCallback(gameObject, player.handleMeleeHeld);
            InputManager.dashButton.addCancelledCallback(gameObject, player.handleDashHeld);
        }
    }
}