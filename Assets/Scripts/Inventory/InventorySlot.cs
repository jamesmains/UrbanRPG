using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,IPointerDownHandler,IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] private VectorVariable playerPositionVariable;
    [field: SerializeField] private GameObject pickupItemObject;
    [field: SerializeField] private Image iconDisplay;
    [field: SerializeField] private TextMeshProUGUI countDisplayText;
    
    [field: SerializeField,FoldoutGroup("Debug"),ReadOnly] private InventoryWindow parentInventoryWindow;
    [field: SerializeField,FoldoutGroup("Debug"),ReadOnly] private static InventorySlot movingItem;
    [field: SerializeField,FoldoutGroup("Debug"),ReadOnly] private bool mouseDown;
    [field: SerializeField,FoldoutGroup("Debug"),ReadOnly] private bool waitingForDrag;
    [field: SerializeField,FoldoutGroup("Debug"),ReadOnly] private Inventory thisInventory;

    [field: SerializeField] public InventoryItemData storedItemData = new(null,0,-1);
    public static InventorySlot highlightedInventorySlot;

    private void OnEnable()
    {
        // GameEvents.OnMouseScroll += TryMoveItemToOtherOpenWindow; // This is REALLY broken
    }

    private void OnDisable()
    {
        movingItem = null;
        GameEvents.OnItemRelease.Raise();
        // GameEvents.OnMouseScroll -= TryMoveItemToOtherOpenWindow;
    }

    private void Update()
    {
        if (waitingForDrag && mouseDown && Vector2.Distance(Input.mousePosition, transform.position) > 85f )
        {
            waitingForDrag = false;
            if (movingItem == null)
            {
                Drag();
            }
        }
        else if (!mouseDown) waitingForDrag = false;
    }

    public void Setup(InventoryItemData inventoryItemData, int index, InventoryWindow origin)
    {
        storedItemData = inventoryItemData;
        storedItemData.Index = index;
        parentInventoryWindow = origin;
        thisInventory = origin.inventory;
        
        if(origin.inventory.InventoryItems[index].item == null || storedItemData.item == null) {Disable(); return; }
        
        iconDisplay.enabled = true;
        iconDisplay.sprite = origin.inventory.InventoryItems[storedItemData.Index].item.Sprite;
        if (origin.inventory.InventoryItems[storedItemData.Index].Quantity > 1)
        {
            countDisplayText.enabled = true;
            countDisplayText.text = origin.inventory.InventoryItems[storedItemData.Index].Quantity.ToString();
        }
        
    }

    public void Disable()
    {
        iconDisplay.enabled = false;
        countDisplayText.enabled = false;
    }

    private void Drag()
    {
        if (parentInventoryWindow.inventory.InventoryItems[storedItemData.Index].Quantity <= 0 || storedItemData.item == null) return;
        movingItem = this;
        GameEvents.OnItemMove.Raise(parentInventoryWindow.inventory.InventoryItems[storedItemData.Index].item);
        iconDisplay.color = new Color(1, 1, 1, 0.5f);
    }
    
    private void DropItem(int q = -1)
    {
        Vector3 spawnPos = playerPositionVariable.Value;
        
        var pickup = Instantiate(pickupItemObject, spawnPos,Quaternion.identity).GetComponent<Pickup>();
        var itemData = parentInventoryWindow.inventory.InventoryItems[storedItemData.Index];
        int quantity = q < 0 ? itemData.Quantity : q;

        pickup.Setup(itemData.item,quantity);
        this.thisInventory.TryRemoveItemAt(storedItemData.Index,quantity);
        if (this.thisInventory.InventoryItems[storedItemData.Index].Quantity <= 0)
            storedItemData = null;
    }

    private void TryReleaseItem()
    {
        if (movingItem == null) return;
        
        if (this != highlightedInventorySlot && highlightedInventorySlot is not null)
        {
            TryMoveItemToSlot();
        }
        else if (InventoryWindow.highlightedInventoryWindow != parentInventoryWindow && InventoryWindow.highlightedInventoryWindow != null)
        {
            TryMoveItemToInventoryWindow(InventoryWindow.highlightedInventoryWindow,storedItemData.Quantity);
        }
        else if(!Global.IsMouseOverUI)
        {
            DropItem();
        }

        movingItem = null;
        iconDisplay.color = new Color(1, 1, 1, 1);
        GameEvents.OnItemRelease.Raise();
        GameEvents.OnMoveOrAddItem.Raise();
    }

    

    private void TryMoveItemToSlot()
    {
        InventorySlot his = highlightedInventorySlot;
        InventoryWindow hisWindow = his.parentInventoryWindow;
                
        if (storedItemData.item == his.storedItemData.item)
            AddItemToExistingStack(hisWindow,his.storedItemData.Index);
        else
            SwapItemStacks(hisWindow,his.storedItemData.Index);  
    }

    private void AddItemToExistingStack(InventoryWindow hisWindow, int hisIndex)
    {
        int quantity = thisInventory.InventoryItems[storedItemData.Index].Quantity;
        var overflow = hisWindow.inventory.TryAddItemAt(hisIndex, quantity);
        thisInventory.TryRemoveItemAt(storedItemData.Index,quantity - overflow);
    }

    private void SwapItemStacks(InventoryWindow hisWindow, int hisIndex)
    {
        if (hisWindow.restrictByItemType &&
            hisWindow.itemTypeRestriction != storedItemData.item.ItemType)
        {
            return;
        }
        thisInventory.TrySwapItem(storedItemData.Index,hisIndex,hisWindow.inventory); 
    }

    private void TryMoveItemToOtherOpenWindow(float movement)
    {
        if (movement == 0 || storedItemData.item == null || movingItem != null) return;
        if (InventoryWindow.openInventoryWindows.Count <= 1 || highlightedInventorySlot == null) return;
        
        int index = InventoryWindow.openInventoryWindows.IndexOf(this.parentInventoryWindow);
        index += (int)Mathf.Abs(movement);
        if (index >= InventoryWindow.openInventoryWindows.Count) index = 0;
        else if (index < 0) index = InventoryWindow.openInventoryWindows.Count - 1; 
        
        print(InventoryWindow.openInventoryWindows.Count);
        
        TryMoveItemToInventoryWindow(InventoryWindow.openInventoryWindows[index], 1);
    }
    
    private void TryMoveItemToInventoryWindow(InventoryWindow targetWindow, int amount)
    {
        if (targetWindow.restrictByItemType &&
            targetWindow.itemTypeRestriction != storedItemData.item.ItemType)
        {
            return;
        }
        var i = parentInventoryWindow.inventory.InventoryItems[storedItemData.Index];
        var overflow = targetWindow.inventory.TryAddItem(i.item,amount);
        parentInventoryWindow.inventory.TryRemoveItemAt(storedItemData.Index,amount - overflow);
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
        if ((highlightedInventorySlot is null || highlightedInventorySlot != this && !parentInventoryWindow.isLocked) && !parentInventoryWindow.removeOnly)
            highlightedInventorySlot = this;
        
        GameEvents.OnMouseEnterInventorySlot.Raise(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedInventorySlot = null;
        GameEvents.OnMouseExitInventorySlot.Raise();
    }

    
}
