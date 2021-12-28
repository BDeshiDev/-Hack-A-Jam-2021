using Core.Input;
using UnityEngine;

namespace Core.Combat.Entities.Player
{
    public class PlayerController:MonoBehaviour
    {
        private HypnoPlayer player;
        private PlayerBombLauncher bomber;
        private void Awake()
        {
            player = GetComponent<HypnoPlayer>();
            bomber = GetComponent<PlayerBombLauncher>();
        }

        private void Start()
        {
            InputManager.Instance.AimOrigin = transform;
            
            InputManager.meleeButton.addPerformedCallback(gameObject, player.handleMeleeHeld);
            InputManager.meleeButton.addCancelledCallback(gameObject, player.handleMeleeReleased);
            InputManager.dashButton.addPerformedCallback(gameObject, player.handleDashHeld);
            InputManager.bombButton.addPerformedCallback(gameObject, bomber.handleBombLaunchAttempt);
        }
    }
}