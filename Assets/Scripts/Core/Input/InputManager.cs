using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using ShieldSan.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{

    public class InputManager : MonoBehaviourSingletonPersistent<InputManager>
    {
        [SerializeField] private InputActionAsset map;
        public Transform AimOrigin;
        [SerializeField] private Camera cam;
        public bool MouseAimActive = false;
        public bool GamePadAimActive = false;
        [SerializeField] private  Vector3 aimDir;   
        public static Vector3 NormalizedTopDownAimInput { get; private set; }
        public static Vector3 NormalizedTopDownAimEndPoint => normalizedTopDownAimEndPoint;
        private static Vector3 normalizedTopDownAimEndPoint;

        // [SerializeField] private LayerMask aimLayer;

        public static Vector3 AimDir => Instance.aimDir;
        public static bool IsAimActive => Instance.aimDir != Vector3.zero;

        [SerializeField]private Vector2 moveInput;
        public static Vector2 RawMoveInput => Instance.moveInput;
        public static Vector3 NormalizedTopDownMoveInput { get; private set; }
        public static bool IsMoveInputActive { get; private set; } = false;


        public bool applySensitivity = true;
        public float gamepadVel = 80;
        private float dotFactor = 3;
        private Vector2 gamepadVal = Vector2.zero;

        [SerializeField]private InputActionReference aimAtAction;
        [SerializeField]private InputActionReference aimAlongAction;
        [SerializeField]private InputActionReference movementAction;
        [SerializeField]private InputActionReference dashAction;
        [SerializeField]private InputActionReference debugAction1;
        [SerializeField]private InputActionReference debugAction2;
        [SerializeField]private InputActionReference debugAction3;

        public static InputButtonSlot dashButton = new InputButtonSlot();
        
        public static InputButtonSlot debugButton1 = new InputButtonSlot();
        public static InputButtonSlot debugButton2 = new InputButtonSlot();
        public static InputButtonSlot debugButton3 = new InputButtonSlot();
        



        // void Update()
        // {
        //     updateAim();
        // }

        protected override void initialize()
        {
            updateCam();
        }

        public void updateCam()
        {
            cam = Camera.main;
        }


        private void AimAlongPerformed(InputAction.CallbackContext obj)
        {
            GamePadAimActive = true;
            MouseAimActive = false;

            gamepadVal = Vector2.zero;
        }
        private void AimAlongCancelled(InputAction.CallbackContext obj)
        {
            GamePadAimActive = false;
        }

        private void OnAimAtPerformed(InputAction.CallbackContext obj)
        {
            MouseAimActive = true;
            GamePadAimActive = false;
        }

        void OnAimAtCancelled(InputAction.CallbackContext c)
        {
            MouseAimActive = false;
        }


        // public void calculateAimPoint(Vector3 playerPos, float maxAimDist, out Vector3 aimEndPoint)
        // {
        //     //#TODO gamepad case
        //     Vector3 aimDirTopDown = NormalizedTopDownAimInput = Vector2.zero;
        //     NormalizedTopDownAimInput = AimOrigin.forward;
        //
        //     var aimDist = maxAimDist;
        //
        //
        //     if (GamePadAimActive)
        //     {
        //         aimDirTopDown = aimAlongAction.action.ReadValue<Vector2>();
        //         if (applySensitivity)
        //         {
        //             float rate = gamepadVel;
        //             var dotProduct = Vector2.Dot(aimDirTopDown, gamepadVal);
        //             if (dotProduct < .5f)//opposite dirs
        //             {
        //                 rate *= dotFactor + (1 - dotProduct);
        //             }
        //
        //             gamepadVal = aimDirTopDown = Vector2.MoveTowards(gamepadVal, aimDirTopDown, Time.deltaTime * rate);
        //         }
        //         aimDist = Mathf.Min(1, aimDirTopDown.magnitude) * maxAimDist;
        //     }
        //     else if (MouseAimActive)
        //     {
        //
        //         var ray = cam.ScreenPointToRay(aimAtAction.action.ReadValue<Vector2>());
        //         Physics.Raycast(ray,out var hit, 1000, aimLayer, QueryTriggerInteraction.Collide);
        //         aimDirTopDown = (hit.point - playerPos);
        //         aimDist = Mathf.Min(aimDirTopDown.magnitude, maxAimDist);
        //     }
        //
        //     aimDirTopDown.y = 0;
        //     NormalizedTopDownAimInput = aimDirTopDown.normalized;
        //
        //
        //     aimEndPoint = (Vector3)playerPos + NormalizedTopDownAimInput * aimDist;
        // }
        //
        //
        // public void updateAim()
        // {
        //     if(AimOrigin != null)
        //         calculateAimPoint(AimOrigin.position, 5, out normalizedTopDownAimEndPoint);
        // }

        void OnEnable()
        {
            if (map == null)
                return;
        
            map.Enable();

            movementAction.action.performed += OnMovePerformed;
            movementAction.action.canceled += OnMoveCancelled ;
            
            dashButton.bind(dashAction);

            #if UNITY_EDITOR
            
            debugButton1.bind(debugAction1);
            debugButton2.bind(debugAction2);
            debugButton3.bind(debugAction3);
            
            #endif
            
            aimAtAction.action.performed += OnAimAtPerformed;
            aimAtAction.action.canceled += OnAimAtCancelled;
            aimAlongAction.action.performed += AimAlongPerformed;
            aimAlongAction.action.canceled += AimAlongCancelled;
        }


        void OnDisable()
        {
            if(map == null)
                return;
            
            movementAction.action.performed -= OnMovePerformed;
            movementAction.action.canceled -= OnMoveCancelled;

            dashButton.unBind(dashAction);

            
#if UNITY_EDITOR
            
            debugButton1.unBind(debugAction1);
            debugButton2.unBind(debugAction2);
            debugButton3.unBind(debugAction3);
            
#endif
            
            aimAtAction.action.performed -= OnAimAtPerformed;
            aimAtAction.action.canceled -= OnAimAtCancelled;
            aimAlongAction.action.performed -= AimAlongPerformed;
            aimAlongAction.action.canceled -= AimAlongCancelled;

            map.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            moveInput = obj.ReadValue<Vector2>();
            NormalizedTopDownMoveInput = moveInput.normalized.toTopDown();
            IsMoveInputActive = true;
        }
        
        private void OnMoveCancelled(InputAction.CallbackContext obj)
        {
            NormalizedTopDownMoveInput = moveInput = Vector2.zero;
            IsMoveInputActive = false;
        }

        private void OnDrawGizmosSelected()
        {
            if(AimOrigin != null)
                Gizmos.DrawRay(AimOrigin.position, aimDir);
        }

        public static void PlayModeExitCleanUp()
        {
            dashButton.cleanup();
            debugButton1.cleanup();
            debugButton1.cleanup();
            debugButton1.cleanup();
        }
    }
}
