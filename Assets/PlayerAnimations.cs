using System;
using FishNet.Component.Animating;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Animator PlayerAnimator;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private NetworkAnimator PlayerNetworkAnimator;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private PlayerMovementState MovementState;

    private void Awake() {
        PlayerAnimator = GetComponent<Animator>();
        PlayerNetworkAnimator = GetComponent<NetworkAnimator>();
    }

    public string GetDirectionName(int rawDirection) => rawDirection switch {
        0 => "South",
        1 => "East",
        2 => "North",
        3 => "West"
    };

    public void SetMovementState(int skin, Vector2 input) {
        string animationLayer = $"Player_Character_{skin}";
        string animationState = "";
        if (input == Vector2.zero) {
            MovementState = PlayerMovementState.Idle;
            animationState = $"Player_Character_{skin}_Idle_{GetDirectionName(0)}";
        }
        else if (input != Vector2.zero) {
            MovementState = PlayerMovementState.Walking;
            animationState = $"Player_Character_{skin}_Walk_{GetDirectionName(0)}";
        }

        PlayerAnimator.SetBool("IsWalking",input != Vector2.zero);
    }
}
