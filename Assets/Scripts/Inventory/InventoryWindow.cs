using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryWindow : Window,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    public Inventory targetInventory;
    [SerializeField] private GameObject InventorySlotPrefab;
    public bool isLocked;
    public bool removeOnly;
    public bool restrictByItemType;
    [ShowIf("restrictByItemType")] public ItemType itemTypeRestriction;

    private bool flaggedToUpdate;
    private List<GameObject> InventorySlots = new();
    public static InventoryWindow highlightedInventoryWindow;

    private void OnEnable()
    {
        GameEvents.OnPickupItem += UpdateInventoryDisplay;
        GameEvents.OnMoveOrAddItem += UpdateInventoryDisplay;
    }

    private void OnDisable()
    {
        GameEvents.OnPickupItem -= UpdateInventoryDisplay;
        GameEvents.OnMoveOrAddItem -= UpdateInventoryDisplay;
    }

    public override void Show()
    {
        base.Show();
        UpdateInventoryDisplay();
    }

    public int AddItem(Item incomingItem, int amount)
    {
        int overflow = targetInventory.TryAddItem(incomingItem, amount);
        GameEvents.OnMoveOrAddItem.Raise();
        return overflow;
    }
    
    public void MoveItem(int originalItemIndex, int targetItemIndex, Inventory otherInventory)
    {
        targetInventory.TrySwapItem(originalItemIndex,targetItemIndex,otherInventory);
    }
    
    [Button]
    public void UpdateInventoryDisplay()
    {
        if (InventorySlots.Count <= 0)
        {
            for (int i = 0; i < targetInventory.InventorySlotLimit.Value; i++)
            {
                InventorySlots.Add(Instantiate(InventorySlotPrefab, this.transform));
            }
        }
        
        for (int i = 0; i < targetInventory.InventorySlotLimit.Value; i++)
        {
            var itemData = targetInventory.InventoryItems[i];
            var slot = InventorySlots[i].GetComponent<InventorySlot>();
            if (restrictByItemType && targetInventory.InventoryItems[i].item != null &&targetInventory.InventoryItems[i].item.ItemType != itemTypeRestriction)
            {
                itemData = new InventoryItemData(null, 0, -1);
                slot.Disable();
            }
            slot.Setup(itemData,i,this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((highlightedInventoryWindow is null || highlightedInventoryWindow != this && !isLocked) && !removeOnly)
            highlightedInventoryWindow = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedInventoryWindow = null;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        
    }
}
