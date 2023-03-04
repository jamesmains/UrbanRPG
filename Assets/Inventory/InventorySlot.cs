using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,IPointerDownHandler,IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [FoldoutGroup("Data")][field: SerializeField] public Item heldItem { get; private set; }
    [FoldoutGroup("Data")][field: SerializeField] public InventoryDisplay parentInventoryDisplay { get; private set; }
    [FoldoutGroup("Data")] [SerializeField] private VectorVariable playerPositionVariable;
    [FoldoutGroup("Data")] [SerializeField] private StringVariable itemNameVariable;
    [FoldoutGroup("Display")][SerializeField] private Image iconDisplay;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI countDisplayText;
    [SerializeField] private GameObject pickupItemObject;


    [SerializeField] [FoldoutGroup("Events")]
    private InventorySlotGameEvent onItemMove;
    [SerializeField] [FoldoutGroup("Events")]
    private InventorySlotGameEvent onItemRelease;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onMouseEnter;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onMouseExit;

    public static InventorySlot movingItem;
    private bool mouseDown;
    private bool waitingForDrag;

    private void Update()
    {
        if (waitingForDrag && Vector2.Distance(Input.mousePosition, (Vector2) this.transform.position) > 85f && mouseDown)
        {
            waitingForDrag = false;
            if (movingItem == null && mouseDown)
            {
                Drag();
            }
        }
        else if (!mouseDown) waitingForDrag = false;
        if(Input.GetMouseButtonUp(0))
        {
            TryReleaseItem();
        }
    }

    public void Setup(Item incomingItem, InventoryDisplay origin)
    {
        heldItem = incomingItem;
        iconDisplay.sprite = heldItem.Sprite;
        countDisplayText.text = origin.targetInventory.ItemList[heldItem].ToString();
        parentInventoryDisplay = origin;
    }

    private void Drag()
    {
        movingItem = this;
        onItemMove.Raise(this);
        iconDisplay.color = new Color(1, 1, 1, 0.5f);
    }

    private void TryMoveItem()
    {
        int o = InventoryDisplay.mouseOverInventoryDisplay.AddItem(heldItem,parentInventoryDisplay.targetInventory.ItemList[heldItem]);
        parentInventoryDisplay.targetInventory.TryUseItem(heldItem,o);
    }
    
    private void DropItem()
    {
        Vector3 spawnPos = playerPositionVariable.Value;
        var pickup = Instantiate(pickupItemObject, spawnPos,Quaternion.identity).GetComponent<Pickup>();
        int quantity = parentInventoryDisplay.targetInventory.ItemList[heldItem];
        parentInventoryDisplay.targetInventory.TryUseItem(heldItem,quantity);
        pickup.Setup(heldItem,quantity);
    }

    private void TryReleaseItem()
    {
        if (movingItem != null)
        {
            if (parentInventoryDisplay != InventoryDisplay.mouseOverInventoryDisplay && InventoryDisplay.mouseOverInventoryDisplay is not null)
            {
                TryMoveItem();
            }
            else if(!parentInventoryDisplay.isMouseOverUserInterface.Value)
            {
                DropItem();
            }
            movingItem = null;
            onItemRelease.Raise(null);
            iconDisplay.color = new Color(1, 1, 1, 1);      
            parentInventoryDisplay.UpdateInventoryDisplay();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
        waitingForDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
        TryReleaseItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseEnter.Raise();
        itemNameVariable.Value = heldItem.Name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseExit.Raise();
    }

    private void OnDisable()
    {
        movingItem = null;
        onItemRelease.Raise(null);
    }
}
