using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [FoldoutGroup("Data")] [SerializeField] private Rigidbody rb;
    [FoldoutGroup("Data")] [SerializeField] private CustoAnimator animator;
    [FoldoutGroup("Data")] [SerializeField] private VectorVariable playerPositionVariable;
    [FoldoutGroup("Data")] [SerializeField] private IntVariable playerLockVariable;
    [FoldoutGroup("Data")] [SerializeField] private PlayerSaveSlot playerSaveSlot;
    [FoldoutGroup("Data")] [SerializeField] public float moveSpeed; // might be replaced with scriptable object float variable 
    
    private float inputX, inputY;
    private bool horizontalFlip;

    private void Awake()
    {
        playerLockVariable.Value = 0;
        if (playerSaveSlot.NextLevelTransition != null && !string.IsNullOrEmpty(playerSaveSlot.NextLevelTransition.TargetScene))
        {
            MovePlayerTo(playerSaveSlot.NextLevelTransition.SpawnLocation);
        }
    }

    private void OnDisable()
    {
        playerSaveSlot.SaveData();
    }

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (playerLockVariable.Value > 0)
        {
            inputX = 0;
            inputY = 0;
        };
        bool isMoving = (inputX != 0 || inputY != 0);
        int action = isMoving ? 1 : 0;
        animator.ChangeDirection(new Vector2((int)inputX,(int)inputY),action);
        Vector3 moveForce = new Vector3(inputX,0, inputY).normalized;
        rb.AddForce(moveForce * moveSpeed);
        playerPositionVariable.Value = transform.position;
    }
    public void MovePlayerTo(Transform newPosition)
    {
        transform.position = newPosition.position;
    }

    public void MovePlayerTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
