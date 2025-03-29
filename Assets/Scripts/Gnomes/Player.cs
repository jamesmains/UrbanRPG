using System;
using System.Collections.Generic;
using Gnomes.Actor.Component;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Gnomes {
    /// <summary>
    /// Possible fix for Local Multiplayer device detection
    /// Get device from InputSystem.onEvent -> Func(InputEventPtr, InputDevice) -> store inputDevice
    /// Check device with SomeCompareFuncViaContext(InputAction.CallbackContext) -> callbackContext.control.device == storedDevice
    ///
    /// Todo: System scope issue:
    /// * Input only considers the features currently implemented in a way that would requires changes
    /// across multiple scripts
    /// I.e. Aim Weapon only considers that the right input and mouse would be used for aiming, but what if it's based on movement?
    /// Updated note (02/23/25):
    /// |-> Currently not finding much issue with this, just needs some minor tweaks in this class
    ///
    /// </summary>
    public class Player : MonoBehaviour {
        [SerializeField, FoldoutGroup("Debug")]
        private float CameraMoveSpeed = 10f;
    
        [SerializeField, FoldoutGroup("Debug")]
        private Actor.Actor DebugTargetActor;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Actor.Actor CurrentActor;
    
        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 MoveInput;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Vector3 LookInput;

        private InputSystem_Actions Input;
        private bool CanMove;

        private void OnEnable() {
            if (Input == null) {
                RegisterNewInputSystem();
            }

            Actor.Actor.OnPossessed += PossessActor;
            Actor.Actor.OnReleasePossession += HandleReleasePossession;
        }
        
        private void OnDisable() {
            if (Input != null) {
                UnregisterInputSystem();
            }

            Actor.Actor.OnPossessed -= PossessActor;
            Actor.Actor.OnReleasePossession -= HandleReleasePossession;
        }

        private void RegisterNewInputSystem() {
            Input = new InputSystem_Actions();
            Input.Enable();
            Input.Player.Move.performed += Move;
            Input.Player.Move.canceled += Move;
            Input.Player.Look.performed += Aim;
            Input.Player.Look.canceled += Aim;
            Input.Player.Attack.performed += Attack;
            Input.Player.Interact.performed += Interact;
        }

        private void UnregisterInputSystem() {
            Input.Player.Move.performed -= Move;
            Input.Player.Move.canceled -= Move;
            Input.Player.Look.performed -= Aim;
            Input.Player.Look.canceled -= Aim;
            Input.Player.Attack.performed -= Attack;
            Input.Player.Interact.performed -= Interact;
            Input.Disable();
            Input = null;
        }

        private void Update() {
            if (!TimeManager.TimeIsRunning) return;
            CurrentActor?.OnMoveActor?.Invoke(MoveInput,true);
            CurrentActor?.OnAimWeapon?.Invoke(LookInput);
            
            // Todo -- Move this to camera
            var targetPosition = CurrentActor ? CurrentActor.transform.position : transform.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * CameraMoveSpeed);
        }

        private void FixedUpdate() {
            // Todo: replace with camera controller if needed
        
        }
        
        [Button]
        private void TryPossess() {
            Actor.Actor.OnTryPossess.Invoke(DebugTargetActor);
        }

        [Button]
        public void TryPossessTarget(Actor.Actor targetActor) {
            DebugTargetActor = targetActor;
            TryPossess();
        }
        
        private void PossessActor(Actor.Actor actor) {
            if (CurrentActor != null) Actor.Actor.OnReleasePossession.Invoke(CurrentActor);
            CurrentActor = actor;
        }

        private void HandleReleasePossession(Actor.Actor releasedActor) {
            if (CurrentActor == releasedActor) {
                CurrentActor = null;
            }
        }

        private void Move(InputAction.CallbackContext callbackContext) {
            var moveDir = (Vector3)callbackContext.ReadValue<Vector2>();
            moveDir.z = moveDir.y;
            moveDir.y = 0;
            MoveInput = moveDir;
        }

        private void Aim(InputAction.CallbackContext callbackContext) {
            LookInput = callbackContext.ReadValue<Vector2>();
        }

        public static Action<InputAction> OnButtonPressed;

        private void Attack(InputAction.CallbackContext callbackContext) {
            CurrentActor?.OnUseWeapon?.Invoke();
            OnButtonPressed?.Invoke(callbackContext.action);
        }

        private void Interact(InputAction.CallbackContext callbackContext) {
            CurrentActor?.OnInteract.Invoke();
        }
    }
}