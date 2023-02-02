using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [field: SerializeField] public Item heldItem { get; private set; }
    [field: SerializeField] public InventoryDisplay originInventory { get; private set; }
    [SerializeField] private GameObject pickupItemObject;
    [SerializeField] private Image iconDisplay;
    [SerializeField] private TextMeshProUGUI countDisplayText;
    [SerializeField] private VectorVariable playerPositionVariable;
    [SerializeField] private InventoryItemManagementGameEvent onItemMove;
    [SerializeField] private InventoryItemManagementGameEvent onItemRelease;

    public static InventorySlot movingItem;
    private bool mouseDown;
    private bool waitingForDrag;

    private void Update()
    {
        //print($"Slot Position: {this.transform.position}, Mouse Position: {Input.mousePosition}, Distance: {Vector2.Distance(Input.mousePosition, (Vector2) this.transform.position)}");
        
        if (waitingForDrag && Vector2.Distance(Input.mousePosition, (Vector2) this.transform.position) > 85f && mouseDown)
        {
            
            waitingForDrag = false;
            if (movingItem == null && mouseDown)
            {
                movingItem = this;
                onItemMove.Raise(this);
            }
        }
        else if (!mouseDown) waitingForDrag = false;
    }

    public void Setup(Item incomingItem, InventoryDisplay origin)
    {
        heldItem = incomingItem;
        iconDisplay.sprite = heldItem.Sprite;
        countDisplayText.text = origin.targetInventory.ItemList[heldItem].ToString();
        originInventory = origin;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
        waitingForDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
        if (movingItem is not null || movingItem != this)
        {
            if (originInventory != InventoryDisplay.mouseOverInventoryDisplay && InventoryDisplay.mouseOverInventoryDisplay is not null)
            {
                int o = InventoryDisplay.mouseOverInventoryDisplay.AddItem(heldItem,originInventory.targetInventory.ItemList[heldItem]);
                originInventory.targetInventory.TryUseItem(heldItem,o);
            }
            else if(!originInventory.isMouseOverUserInterface.Value)
            {
                Vector3 spawnPos = playerPositionVariable.Value;
                var pickup = Instantiate(pickupItemObject, spawnPos,Quaternion.identity).GetComponent<Pickup>();
                int quantity = originInventory.targetInventory.ItemList[heldItem];
                originInventory.targetInventory.TryUseItem(heldItem,quantity);
                pickup.Setup(heldItem,quantity);
            }
            movingItem = null;
            onItemRelease.Raise(null);
            iconDisplay.color = new Color(1, 1, 1, 1);      
            originInventory.UpdateInventoryDisplay();
        }
        
    }
}
