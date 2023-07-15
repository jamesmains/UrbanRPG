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
    private bool movingLeft,movingRight,movingUp,movingDown;
    private bool horizontalFlip;

    private bool isRunning;

    private void Awake()
    {
        playerLockVariable.Value = 0;
        if (playerSaveSlot.NextLevelTransition != null && !string.IsNullOrEmpty(playerSaveSlot.NextLevelTransition.TargetScene))
        {
            MovePlayerTo(playerSaveSlot.NextLevelTransition.SpawnLocation);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnMoveLeftButtonHeld += delegate { movingLeft = true; };
        GameEvents.OnMoveLeftButtonReleased += delegate { movingLeft = false; };
        
        GameEvents.OnMoveRightButtonHeld += delegate { movingRight = true; };
        GameEvents.OnMoveRightButtonReleased += delegate { movingRight = false; };
        
        GameEvents.OnMoveUpButtonHeld += delegate { movingUp = true; };
        GameEvents.OnMoveUpButtonReleased += delegate { movingUp = false; };
        
        GameEvents.OnMoveDownButtonHeld += delegate { movingDown = true; };
        GameEvents.OnMoveDownButtonReleased += delegate { movingDown = false; };
    }

    private void OnDisable()
    {
        GameEvents.OnMoveLeftButtonHeld -= delegate { movingLeft = true; };
        GameEvents.OnMoveLeftButtonReleased -= delegate { movingLeft = false; };
        
        GameEvents.OnMoveRightButtonHeld -= delegate { movingRight = true; };
        GameEvents.OnMoveRightButtonReleased -= delegate { movingRight = false; };
        
        GameEvents.OnMoveUpButtonHeld -= delegate { movingUp = true; };
        GameEvents.OnMoveUpButtonReleased -= delegate { movingUp = false; };
        
        GameEvents.OnMoveDownButtonHeld -= delegate { movingDown = true; };
        GameEvents.OnMoveDownButtonReleased -= delegate { movingDown = false; };
        
        playerSaveSlot.SaveData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
        
        inputX = 0;
        if (movingLeft)
            inputX -= 1;
        if (movingRight)
            inputX += 1;
        inputY = 0;
        if (movingDown)
            inputY -= 1;
        if (movingUp)
            inputY += 1;
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
        if (isRunning) action = 2;
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
        print($"Spawn position: {newPosition}");
        transform.position = newPosition;
    }
}
