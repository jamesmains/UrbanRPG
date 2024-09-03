using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FishNet;
using FishNet.Component.Animating;
using FishNet.Managing.Scened;
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
    private List<Interactable> OverlappedInteractableObjects;
    
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
    private Chatbox PlayerChatbox;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private InputSystem_Actions Input;

    public override void OnStartClient() {
        base.OnStartClient();
        if (!IsOwner) return;
        if (Input == null) {
            Input = new InputSystem_Actions();
            Input.Enable();
        }
        PlayerAnimations = GetComponent<PlayerAnimations>();
        PlayerChatbox = GameObject.FindAnyObjectByType<Chatbox>();
        Input.UI.Submit.performed += delegate { PlayerChatbox.TryFocusInputField(); };
        Input.Player.Interact.performed += TryInteract;

        if (CurrentPlayerInfo.Data == null) {
            CurrentPlayerInfo.Data = new PlayerData {
                PlayerName = "Guest", // This shouldn't normally be possible
                UniqueId = Guid.NewGuid().ToString()
            };
        }

        // This should be replaced by some server message that only this player sees, seeing when ANYONE joins
        // could render the chat channel useless
        PlayerChatbox.SendMessage(new Message() {
            
            username = CurrentPlayerInfo.Data.PlayerName,
            message = "Has Entered the City!"
        });
        FindAnyObjectByType<PlayerCanvas>().HookToPlayer();

        StartCoroutine(Test());
        IEnumerator Test() {
            yield return new WaitForSeconds(1);
            Inventory.Singleton.LoadInventory();
        }
    }

    private void OnConnectedToServer() {
        
    }

    public override void OnStopClient() {
        base.OnStopClient();
        if (!IsOwner) return;
        PlayerChatbox.Disable();
        if (Input != null) {
            Input.Disable();
            Input.UI.Submit.performed -= delegate { PlayerChatbox.TryFocusInputField(); };
        }
    }
    
    private void OnEnable() {
        if (!IsOwner) return;
    }

    private void OnDisable() {
        if (!IsOwner) return;
    }


    private void Update() {
        if (!IsOwner) return;
        if (PlayerChatbox.IsFocussedOnText()) return;
        CurrentMovementInput = Input.Player.Move.ReadValue<Vector2>();
        PlayerAnimations.SetMovementState(CharacterSkin,CurrentMovementInput);
        MovementLean();
        transform.position += (Vector3)CurrentMovementInput.normalized * (MoveSpeed * Time.deltaTime);
    }

    private void TryInteract(InputAction.CallbackContext callbackContext) {
        if (!IsOwner) return;
        if(OverlappedInteractableObjects.Count > 0)
            OverlappedInteractableObjects[0].Interact();
    }

    private void MovementLean() {
        float zRot = CurrentMovementInput.x > 0 ? -7 : CurrentMovementInput.x < 0 ? 7 : 0;
        transform.DORotate(new Vector3(0, 0, zRot), 0, RotateMode.Fast);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!IsOwner) return;
        if (other.TryGetComponent(out Interactable i)) {
            if (OverlappedInteractableObjects.Contains(i)) return;
            OverlappedInteractableObjects.Add(i);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!IsOwner) return;
        if (other.TryGetComponent(out Interactable i)) {
            if (OverlappedInteractableObjects.Contains(i))
                OverlappedInteractableObjects.Remove(i);
        }
    }
}
