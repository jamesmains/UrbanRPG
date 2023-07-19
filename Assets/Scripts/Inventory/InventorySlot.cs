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
        GameEvents.OnMouseScroll += TryMoveItemToOtherOpenWindow;
        GameEvents.OnAltMouseButtonDown += TryConsumeItem;
    }

    private void OnDisable()
    {
        movingItem = null;
        GameEvents.OnItemRelease.Raise();
        GameEvents.OnMouseScroll -= TryMoveItemToOtherOpenWindow;
        GameEvents.OnAltMouseButtonDown -= TryConsumeItem;
    }

    private void Update()
    {
        if (parentInventoryWindow == null || parentInventoryWindow.isLocked) return;
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
        
        if (inventoryItemData.Item is Gear item && inventoryItemData != storedItemData && origin.restrictByItemType)
        {
            if (inventoryItemData.Item.ItemType == origin.itemTypeRestriction)
            {
                foreach (var effect in item.GearEffects)
                {
                    effect.OnEquip();
                }
            }
            
        }
        
        storedItemData = inventoryItemData;
        storedItemData.Index = index;
        parentInventoryWindow = origin;
        thisInventory = origin.inventory;

        if(origin.inventory.InventoryItems[index].Item == null || storedItemData.Item == null) {Disable(); return; }
        
        iconDisplay.enabled = true;
        iconDisplay.sprite = origin.inventory.InventoryItems[storedItemData.Index].Item.Sprite;
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
        if (parentInventoryWindow.inventory.InventoryItems[storedItemData.Index].Quantity <= 0 || storedItemData.Item == null) return;
        movingItem = this;
        GameEvents.OnItemMove.Raise(parentInventoryWindow.inventory.InventoryItems[storedItemData.Index].Item);
        iconDisplay.color = new Color(1, 1, 1, 0.5f);
    }
    
    private void DropItem(int q = -1)
    {
        Vector3 spawnPos = playerPositionVariable.Value;
        
        var pickup = Instantiate(pickupItemObject, spawnPos,Quaternion.identity).GetComponent<Pickup>();
        var itemData = parentInventoryWindow.inventory.InventoryItems[storedItemData.Index];
        int quantity = q < 0 ? itemData.Quantity : q;

        pickup.Setup(itemData.Item,quantity);
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

    private void TryConsumeItem()
    {
        if (highlightedInventorySlot != this) return;
        if (storedItemData.Item.IsConsumable)
        {
            foreach (var itemEffect in storedItemData.Item.ItemEffects)
            {
                itemEffect.OnConsume();   
            }
            thisInventory.TryRemoveItemAt(storedItemData.Index,1);
        }
    }

    private void TryMoveItemToSlot()
    {
        InventorySlot his = highlightedInventorySlot;
        InventoryWindow hisWindow = his.parentInventoryWindow;

        if (hisWindow.removeOnly) return;
        
        if (storedItemData.Item == his.storedItemData.Item)
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
            hisWindow.itemTypeRestriction != storedItemData.Item.ItemType)
        {
            return;
        }
        thisInventory.TrySwapItem(storedItemData.Index,hisIndex,hisWindow.inventory); 
    }

    private void TryMoveItemToOtherOpenWindow(float movement)
    {
        if (movement == 0 || storedItemData.Item == null || movingItem != null) return;
        if (InventoryWindow.openInventoryWindows.Count <= 1 || highlightedInventorySlot != this) return;
        
        int index = InventoryWindow.openInventoryWindows.IndexOf(this.parentInventoryWindow);
        index += (int)Mathf.Abs(movement) * movement > 0 ? 1 : -1;
        if (index >= InventoryWindow.openInventoryWindows.Count) index = 0;
        else if (index < 0) index = InventoryWindow.openInventoryWindows.Count - 1; 
        
        if(movement > 0)
            TryMoveItemToInventoryWindow(InventoryWindow.openInventoryWindows[index], 1);
        else if(movement < 1)
            TryGetItemFromInventoryWindow(InventoryWindow.openInventoryWindows[index], 1);
    }
    
    private void TryMoveItemToInventoryWindow(InventoryWindow targetWindow, int amount)
    {
        if ((targetWindow.restrictByItemType &&
            targetWindow.itemTypeRestriction != storedItemData.Item.ItemType) || targetWindow.removeOnly)
        {
            return;
        }
        var i = parentInventoryWindow.inventory.InventoryItems[storedItemData.Index];
        var overflow = targetWindow.inventory.TryAddItem(i.Item,amount);
        parentInventoryWindow.inventory.TryRemoveItemAt(storedItemData.Index,amount - overflow);
    }

    private void TryGetItemFromInventoryWindow(InventoryWindow targetWindow, int amount)
    {
        if (targetWindow.restrictByItemType &&
            targetWindow.itemTypeRestriction != storedItemData.Item.ItemType)
        {
            return;
        }
        var i = targetWindow.inventory.HasItemAt(storedItemData.Item);
        if (i < 0) return;
        var overflow = parentInventoryWindow.inventory.TryAddItem(storedItemData.Item,amount);
        targetWindow.inventory.TryRemoveItemAt(i,amount - overflow);
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
        if ((highlightedInventorySlot is null || highlightedInventorySlot != this) && !parentInventoryWindow.isLocked)
            highlightedInventorySlot = this;
        
        GameEvents.OnMouseEnterInventorySlot.Raise(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedInventorySlot = null;
        GameEvents.OnMouseExitInventorySlot.Raise();
    }

    
}
