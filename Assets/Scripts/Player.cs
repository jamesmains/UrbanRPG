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

    [SerializeField] [FoldoutGroup("Dependencies")]
    private Animator PlayerAnimator;
    
    [SerializeField] [FoldoutGroup("Settings")]
    private float MoveSpeed;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int FacingDirection; // 0 == South, 1 == East, 2 == North, 3 == West
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int CharacterSkin; // 0 - 7
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Vector2 CurrentMovementInput;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private PlayerMovementState MovementState;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private InputSystem_Actions Input;

    public override void OnStartClient() {
        base.OnStartClient();
        if (Input == null) {
            Input = new InputSystem_Actions();
            Input.Enable();
        }
    }

    public override void OnStopClient() {
        base.OnStopClient();
        if (Input != null)
            Input.Disable();
    }

    private void Update() {
        if (!IsOwner) return;
        CurrentMovementInput = Input.Player.Move.ReadValue<Vector2>();
        SetMovementState();
        MovementLean();
        transform.position += (Vector3)CurrentMovementInput.normalized * (MoveSpeed * Time.deltaTime);
    }

    private void MovementLean() {
        float zRot = CurrentMovementInput.x > 0 ? -7 : CurrentMovementInput.x < 0 ? 7 : 0;
        transform.DORotate(new Vector3(0, 0, zRot), 0, RotateMode.Fast);
    }

    public string GetDirectionName(int rawDirection) => rawDirection switch {
        0 => "South",
        1 => "East",
        2 => "North",
        3 => "West"
    };

    private void SetMovementState() {
        string animationLayer = $"Player_Character_{CharacterSkin}";
        string animationState = "";
        if (CurrentMovementInput == Vector2.zero) {
            MovementState = PlayerMovementState.Idle;
            animationState = $"Player_Character_{CharacterSkin}_Idle_{GetDirectionName(FacingDirection)}";
        }
        else if (CurrentMovementInput != Vector2.zero) {
            MovementState = PlayerMovementState.Walking;
            animationState = $"Player_Character_{CharacterSkin}_Walk_{GetDirectionName(FacingDirection)}";
        }

        print(animationState);
        PlayerAnimator.Play($"{animationLayer}.{animationState}");
    }
}
