using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class PlayerMotor : MonoBehaviour
    {
        public Inventory RideGearInventory;
        public GameObject RidingIndicator;
    
        [FoldoutGroup("Data")] [SerializeField] private Rigidbody rb;
        [FoldoutGroup("Data")] [SerializeField] private CustoAnimator animator;
        [FoldoutGroup("Data")] [SerializeField] private Vector2 playerPositionVariable; // Todo - replace with static variable
        [FoldoutGroup("Data")] [SerializeField] private PlayerSaveSlot playerSaveSlot;
        [FoldoutGroup("Data")] [SerializeField] public ModdableFloat moveSpeed; // might be replaced with scriptable object float variable 
    
        private float inputX, inputY;
        private bool movingLeft,movingRight,movingUp,movingDown;
        private bool horizontalFlip;
        private bool isRiding;
        private bool isRunning;

        private void Awake()
        {
            Global.PlayerLock = 0;
            if (playerSaveSlot.NextSceneTransition != null && !string.IsNullOrEmpty(playerSaveSlot.NextSceneTransition.TargetScene))
            {
                MovePlayerTo(playerSaveSlot.NextSceneTransition.SpawnLocation);
            }
        }

        private void OnEnable()
        {
        
            UpdateMoveSpeed();
        }

        private void OnDisable()
        {
        
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
            GameEvents.OnPlayerMoved.Invoke();
            UpdateMoveSpeed();
        }

        [Button]
        private void DismountRide()
        {
            isRiding = false;
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
            if (Global.PlayerLock > 0)
            {
                inputX = 0;
                inputY = 0;
            };
            bool isMoving = (inputX != 0 || inputY != 0);
            int action = isMoving ? 1 : 0;
            if (isRunning) action = 2;
            if (isRiding) action = 0;
            animator.ChangeDirection(new Vector2((int)inputX,(int)inputY),action);
            Vector3 moveForce = new Vector3(inputX,0, inputY).normalized;
            rb.AddForce(moveForce * moveSpeed.Value);
            playerPositionVariable = transform.position;
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
}
