using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryWindow : Window,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    public Inventory inventory;
    [SerializeField] private GameObject InventorySlotPrefab;
    public bool isLocked;
    public bool removeOnly;
    public bool restrictByItemType;
    [ShowIf("restrictByItemType")] public ItemType itemTypeRestriction;

    private bool flaggedToUpdate;
    private List<GameObject> InventorySlots = new();
    public static InventoryWindow highlightedInventoryWindow;
    public static List<InventoryWindow> openInventoryWindows = new();

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
        openInventoryWindows.Add(this);
    }

    public override void Hide()
    {
        base.Hide();
        openInventoryWindows.Remove(this);
    }

    [Button]
    public void UpdateInventoryDisplay()
    {
        if (InventorySlots.Count <= 0)
        {
            for (int i = 0; i < inventory.InventorySlotLimit.Value; i++)
            {
                InventorySlots.Add(Instantiate(InventorySlotPrefab, this.transform));
            }
        }
        
        for (int i = 0; i < inventory.InventorySlotLimit.Value; i++)
        {
            var itemData = inventory.InventoryItems[i];
            var slot = InventorySlots[i].GetComponent<InventorySlot>();
            if (restrictByItemType && inventory.InventoryItems[i].item != null &&inventory.InventoryItems[i].item.ItemType != itemTypeRestriction)
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
