using System;
using BDeshi.Utility;
using BDeshi.Utility.Extensions;
using ShieldSan.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        [SerializeField] private  Vector3 aimPoint;   
        public static Vector3 NormalizedAimDir { get; private set; }

        // [SerializeField] private LayerMask aimLayer;

        public static Vector3 AimDir => Instance.aimDir;
        public static Vector3 AimPoint => Instance.aimPoint;
        public static bool IsAimActive => Instance.aimDir != Vector3.zero;
        

        [SerializeField]private Vector2 moveInput;
        public static Vector2 RawMoveInput => Instance.moveInput;
        public static Vector3 NormalizedMoveInput { get; private set; }
        public static bool IsMoveInputActive { get; private set; } = false;


        public bool applySensitivity = true;
        public float gamepadVel = 80;
        private float dotFactor = 3;
        private Vector2 gamepadVal = Vector2.zero;

        [SerializeField]private InputActionReference aimAtAction;
        [SerializeField]private InputActionReference aimAlongAction;
        [SerializeField]private InputActionReference movementAction;
        [SerializeField]private InputActionReference dashAction;
        [SerializeField]private InputActionReference meleeAction;
        [SerializeField]private InputActionReference bombAction;
        
        [SerializeField]private InputActionReference debugAction1;
        [SerializeField]private InputActionReference debugAction2;
        [SerializeField]private InputActionReference debugAction3;


        public static InputButtonSlot meleeButton = new InputButtonSlot();
        public static InputButtonSlot bombButton = new InputButtonSlot();
        public static InputButtonSlot dashButton = new InputButtonSlot();

        
        public static InputButtonSlot debugButton1 = new InputButtonSlot();
        public static InputButtonSlot debugButton2 = new InputButtonSlot();
        public static InputButtonSlot debugButton3 = new InputButtonSlot();


        private void Start()
        {

            if (GameStateManager.Instance!= null)
            {
                
                if(GameStateManager.Instance.IsPaused)
                    HandlePaused();
                else
                    handleUnPaused();
                
                GameStateManager.Instance.Paused += HandlePaused;
                GameStateManager.Instance.UnPaused += handleUnPaused;
            }
        }

        private void OnDestroy()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.Paused -= HandlePaused;
                GameStateManager.Instance.UnPaused -= handleUnPaused;
            }
        }

        private void HandlePaused()
        {
            // Debug.Log("pause input");
            map.FindActionMap("Gameplay").Disable();
        }

        private void handleUnPaused()
        {
            // Debug.Log("resume input");

            map.FindActionMap("Gameplay").Enable();
        }


        void Update()
        {
            if(GameStateManager.Instance.IsPaused)
            {
                return;
            }
            updateAim();
        }

        protected override void initialize()
        {
            updateCam();
        }
        

    
        private void AimAlongPerformed(InputAction.CallbackContext obj)
        {
            GamePadAimActive = true;
            MouseAimActive = false;

            gamepadVal = Vector2.zero;
        }
        
        public void updateCam()
        {
            cam = Camera.main;
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


        public void calculateAimPoint(Vector3 playerPos, float maxAimDist, out Vector3 aimEndPoint)
        {
            //#TODO gamepad case
            aimDir = NormalizedAimDir = Vector2.zero;
            NormalizedAimDir = AimOrigin.up;
        
            var aimDist = maxAimDist;
            
        
            if (GamePadAimActive)
            {
                aimDir = aimAlongAction.action.ReadValue<Vector2>();
                if (applySensitivity)
                {
                    float rate = gamepadVel;
                    var dotProduct = Vector2.Dot(aimDir, gamepadVal);
                    if (dotProduct < .5f)//opposite dirs
                    {
                        rate *= dotFactor + (1 - dotProduct);
                    }
        
                    gamepadVal = aimDir = Vector2.MoveTowards(gamepadVal, aimDir, Time.deltaTime * rate);
                }
                aimDist = Mathf.Min(1, aimDir.magnitude) * maxAimDist;
                aimEndPoint = (Vector3)playerPos + NormalizedAimDir * aimDist;
            }
            else if (MouseAimActive)
            {
                aimEndPoint = cam.ScreenToWorldPoint(aimAtAction.action.ReadValue<Vector2>());
                aimDir = (aimEndPoint - playerPos);
            }
            else
            {
                aimEndPoint = playerPos + NormalizedAimDir * aimDist;
            }

            aimDir.z = 0;
            aimEndPoint.z = 0;
            NormalizedAimDir = aimDir.normalized;
        
        
        }
        
        
        public void updateAim()
        {
            if(cam == null)
                updateCam();
            if(cam != null && AimOrigin != null)
                calculateAimPoint(AimOrigin.position, 5, out aimPoint);
        }

        void OnEnable()
        {
            if (map == null)
                return;
        
            map.Enable();

            movementAction.action.performed += OnMovePerformed;
            movementAction.action.canceled += OnMoveCancelled ;
            
            meleeButton.bind(meleeAction);
            bombButton.bind(bombAction);
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

            meleeButton.unBind(meleeAction);
            bombButton.unBind(bombAction);
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
            NormalizedMoveInput = moveInput.normalized;
            IsMoveInputActive = true;
        }
        
        private void OnMoveCancelled(InputAction.CallbackContext obj)
        {
            NormalizedMoveInput = moveInput = Vector2.zero;
            IsMoveInputActive = false;
        }

        private void OnDrawGizmosSelected()
        {
            if(AimOrigin != null)
            {
                // Gizmos.color = Color.green;
                // Gizmos.DrawRay(AimOrigin.position, aimDir);
                // Gizmos.color = Color.red;

                Gizmos.DrawLine(AimOrigin.position, aimPoint);
            }
        }

        public static void PlayModeExitCleanUp()
        {
            meleeButton.cleanup();
            dashButton.cleanup();
            bombButton.cleanup();
            
            debugButton1.cleanup();
            debugButton1.cleanup();
            debugButton1.cleanup();
        }
    }
}
