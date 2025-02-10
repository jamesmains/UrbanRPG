using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugPlayer : MonoBehaviour {
    [SerializeField, FoldoutGroup("Settings")]
    private float MoveSpeed;

    [SerializeField, FoldoutGroup("Dependencies")]
    private Rigidbody Rb;
    
    [SerializeField, FoldoutGroup("Dependencies")]
    private Animator PlayerAnimator;
    
    [SerializeField, FoldoutGroup("Status"),ReadOnly]
    private Vector2 MoveVector;

    private void Update() {
        MoveVector.x = Input.GetAxis("Horizontal"); 
        MoveVector.y = Input.GetAxis("Vertical"); 
    }

    private void FixedUpdate() {
        var moveDir = (Vector3)MoveVector;
        moveDir.z = moveDir.y;
        moveDir.y = 0;
        Rb.AddForce(moveDir * MoveSpeed);
        PlayerAnimator.SetBool("IsMoving", moveDir != Vector3.zero);
    }
}
