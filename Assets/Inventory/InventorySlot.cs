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
    [field: SerializeField] public Item heldItem { get; private set; }
    [field: SerializeField] public InventoryDisplay originInventory { get; private set; }
    [SerializeField] private GameObject pickupItemObject;
    [SerializeField] private Image iconDisplay;
    [SerializeField] private TextMeshProUGUI countDisplayText;
    [SerializeField] private VectorVariable playerPositionVariable;
    
    [SerializeField] [FoldoutGroup("Events")]
    private InventoryItemManagementGameEvent onItemMove;
    [SerializeField] [FoldoutGroup("Events")]
    private InventoryItemManagementGameEvent onItemRelease;

    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onMouseEnter;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onMouseExit;
    
    [SerializeField] private StringVariable itemNameVariable;

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
    }

    public void Setup(Item incomingItem, InventoryDisplay origin)
    {
        heldItem = incomingItem;
        iconDisplay.sprite = heldItem.Sprite;
        countDisplayText.text = origin.targetInventory.ItemList[heldItem].ToString();
        originInventory = origin;
    }

    private void Drag()
    {
        movingItem = this;
        onItemMove.Raise(this);
        iconDisplay.color = new Color(1, 1, 1, 0.5f);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseEnter.Raise();
        itemNameVariable.Value = heldItem.Name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseExit.Raise();
    }
}
