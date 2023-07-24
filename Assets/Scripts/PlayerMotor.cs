using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public Inventory RideGearInventory;
    public GameObject RidingIndicator;
    
    [FoldoutGroup("Data")] [SerializeField] private Rigidbody rb;
    [FoldoutGroup("Data")] [SerializeField] private CustoAnimator animator;
    [FoldoutGroup("Data")] [SerializeField] private VectorVariable playerPositionVariable;
    [FoldoutGroup("Data")] [SerializeField] private IntVariable playerLockVariable;
    [FoldoutGroup("Data")] [SerializeField] private PlayerSaveSlot playerSaveSlot;
    [FoldoutGroup("Data")] [SerializeField] public ModdableFloat moveSpeed; // might be replaced with scriptable object float variable 
    
    private float inputX, inputY;
    private bool movingLeft,movingRight,movingUp,movingDown;
    private bool horizontalFlip;
    private bool isRiding;
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
        
        GameEvents.OnRideButtonDown += ToggleRide;
        GameEvents.OnMoveOrAddItem += UpdateMoveSpeed;
        GameEvents.OnChangeRide += DismountRide;
        
        UpdateMoveSpeed();
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
        
        GameEvents.OnRideButtonDown -= ToggleRide;
        GameEvents.OnMoveOrAddItem -= UpdateMoveSpeed;
        GameEvents.OnChangeRide -= DismountRide;
        
        playerSaveSlot.SaveData();
    }

    private void ToggleRide()
    {
        if (isRiding)
        {
            DismountRide();
        }   
        else MountRide();
    }
    
    [Button]
    private void MountRide()
    {
        isRiding = RideGearInventory.InventoryItems[0].Item != null;
        GameEvents.OnPlayerMoved.Raise();
        UpdateMoveSpeed();
    }

    [Button]
    private void DismountRide()
    {
        isRiding = false;
        GameEvents.OnPlayerMoved.Raise();
        UpdateMoveSpeed();
    }
    
    [Button]
    private void UpdateMoveSpeed()
    {
        moveSpeed.ModValues.Clear();
        RidingIndicator.gameObject.SetActive(isRiding);
        isRiding = RideGearInventory.InventoryItems[0].Item != null && isRiding;
        
        if (!isRiding) return;
        
        foreach (var gear in RideGearInventory.InventoryItems)
        {
            if (gear.Item is not Gear item) continue;
            foreach (var effect in item.GearEffects)
            {
                if (effect is not RideEffect rideEffect) continue;
                moveSpeed.ModValues.Add(rideEffect.GetEffectValue());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // Just for testing
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
        if (isRiding) action = 0;
        if(isMoving) GameEvents.OnPlayerMoved.Raise();
        animator.ChangeDirection(new Vector2((int)inputX,(int)inputY),action);
        Vector3 moveForce = new Vector3(inputX,0, inputY).normalized;
        rb.AddForce(moveForce * moveSpeed.Value);
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
