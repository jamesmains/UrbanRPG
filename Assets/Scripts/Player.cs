using System;
using DG.Tweening;
using FishNet.Component.Animating;
using FishNet.Object;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public enum PlayerMovementState{
    Idle,
    Walking,
    Surfing,
    Skating
}

public class Player : NetworkBehaviour {
    
    [SerializeField] [FoldoutGroup("Settings")]
    private float MoveSpeed;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int FacingDirection; // 0 == South, 1 == East, 2 == North, 3 == West
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int CharacterSkin; // 0 - 7
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Vector2 CurrentMovementInput;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private PlayerAnimations PlayerAnimations;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int Locks;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private InputSystem_Actions Input;

    public override void OnStartClient() {
        base.OnStartClient();
        if (Input == null) {
            Input = new InputSystem_Actions();
            Input.Enable();
        }
        // Chatbox.Singleton.OnSendMessage.Invoke($"{LocalConnection.ClientId} Has Entered the City!");
        PlayerAnimations = GetComponent<PlayerAnimations>();
        // Input.UI.Submit.performed += delegate { Chatbox.Singleton.TryFocusInputField(); };
    }

    public override void OnStopClient() {
        base.OnStopClient();
        if (Input != null)
            Input.Disable();
        // Input.UI.Submit.performed -= delegate { Chatbox.Singleton.TryFocusInputField(); };
    }

    
    
    private void Update() {
        if (!IsOwner) return;
        // if (Chatbox.Singleton.IsFocussedOnText()) return;
        CurrentMovementInput = Input.Player.Move.ReadValue<Vector2>();
        PlayerAnimations.SetMovementState(CharacterSkin,CurrentMovementInput);
        MovementLean();
        transform.position += (Vector3)CurrentMovementInput.normalized * (MoveSpeed * Time.deltaTime);
    }

    private void MovementLean() {
        float zRot = CurrentMovementInput.x > 0 ? -7 : CurrentMovementInput.x < 0 ? 7 : 0;
        transform.DORotate(new Vector3(0, 0, zRot), 0, RotateMode.Fast);
    }

    
}
