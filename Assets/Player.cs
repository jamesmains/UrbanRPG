using System;
using FishNet.Object;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour {
    
    [SerializeField] [FoldoutGroup("Settings")]
    private float MoveSpeed;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Vector2 CurrentMovementInput;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private InputSystemActions Input;

    public override void OnStartClient() {
        base.OnStartClient();
        if (Input == null) {
            Input = new InputSystemActions();
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
        transform.position += (Vector3)CurrentMovementInput * MoveSpeed * Time.deltaTime;
    }
}
