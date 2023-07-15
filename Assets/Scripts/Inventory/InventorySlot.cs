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

    [field: SerializeField] public InventoryItemData storedItem =new(null,0,-1);
    public static InventorySlot highlightedInventorySlot;
    
    private void OnDisable()
    {
        movingItem = null;
        GameEvents.OnItemRelease.Raise();
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
        storedItem = inventoryItemData;
        storedItem.Index = index;
        parentInventoryWindow = origin;
        thisInventory = origin.inventory;
        
        if(origin.inventory.InventoryItems[index].item == null || storedItem.item == null) {Disable(); return; }
        
        iconDisplay.enabled = true;
        iconDisplay.sprite = origin.inventory.InventoryItems[storedItem.Index].item.Sprite;
        countDisplayText.enabled = true;
        countDisplayText.text = origin.inventory.InventoryItems[storedItem.Index].Quantity.ToString();
    }

    public void Disable()
    {
        iconDisplay.enabled = false;
        countDisplayText.enabled = false;
    }

    private void Drag()
    {
        if (parentInventoryWindow.inventory.InventoryItems[storedItem.Index].Quantity <= 0 || storedItem.item == null) return;
        movingItem = this;
        GameEvents.OnItemMove.Raise(parentInventoryWindow.inventory.InventoryItems[storedItem.Index].item);
        iconDisplay.color = new Color(1, 1, 1, 0.5f);
    }
    
    private void DropItem(int q = -1)
    {
        Vector3 spawnPos = playerPositionVariable.Value;
        
        var pickup = Instantiate(pickupItemObject, spawnPos,Quaternion.identity).GetComponent<Pickup>();
        var itemData = parentInventoryWindow.inventory.InventoryItems[storedItem.Index];
        int quantity = q < 0 ? itemData.Quantity : q;

        pickup.Setup(itemData.item,quantity);
        this.thisInventory.TryRemoveItemAt(storedItem.Index,quantity);
        if (this.thisInventory.InventoryItems[storedItem.Index].Quantity <= 0)
            storedItem = null;
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
            TryMoveItemToInventoryWindow();
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
                
        if (storedItem.item == his.storedItem.item)
            AddItemToExistingStack(hisWindow,his.storedItem.Index);
        else
            SwapItemStacks(hisWindow,his.storedItem.Index);  
    }

    private void AddItemToExistingStack(InventoryWindow hisWindow, int hisIndex)
    {
        int quantity = thisInventory.InventoryItems[storedItem.Index].Quantity;
        var overflow = hisWindow.inventory.TryAddItemAt(hisIndex, quantity);
        thisInventory.TryRemoveItemAt(storedItem.Index,quantity - overflow);
    }

    private void SwapItemStacks(InventoryWindow hisWindow, int hisIndex)
    {
        if (hisWindow.restrictByItemType &&
            hisWindow.itemTypeRestriction != storedItem.item.ItemType)
        {
            return;
        }
        thisInventory.TrySwapItem(storedItem.Index,hisIndex,hisWindow.inventory); 
    }

    private void TryMoveItemToInventoryWindow()
    {
        if (InventoryWindow.highlightedInventoryWindow.restrictByItemType &&
            InventoryWindow.highlightedInventoryWindow.itemTypeRestriction != storedItem.item.ItemType)
        {
            return;
        }
        var i = parentInventoryWindow.inventory.InventoryItems[storedItem.Index];
        var overflow = InventoryWindow.highlightedInventoryWindow.inventory.TryAddItem(i.item,i.Quantity);
        parentInventoryWindow.inventory.TryRemoveItemAt(storedItem.Index,i.Quantity - overflow);
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
