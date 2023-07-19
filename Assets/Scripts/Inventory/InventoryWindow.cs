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
    public List<InventoryWindow> DEBUGopenInventoryWindows = new();

    private void OnEnable()
    {
        PopulateDisplay();
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
        DEBUGopenInventoryWindows = openInventoryWindows;
    }

    public override void Hide()
    {
        base.Hide();
        openInventoryWindows.Remove(this);
        DEBUGopenInventoryWindows = openInventoryWindows;
    }

    private void PopulateDisplay()
    {
        if (InventorySlots.Count <= 0)
        {
            for (int i = 0; i < inventory.InventorySlotLimit.Value; i++)
            {
                InventorySlots.Add(Instantiate(InventorySlotPrefab, this.transform));
            }
        }
    }
    
    [Button]
    public void UpdateInventoryDisplay()
    {
        PopulateDisplay(); // Should only run correctly from editor
        for (int i = 0; i < inventory.InventorySlotLimit.Value; i++)
        {
            var itemData = inventory.InventoryItems[i];
            var slot = InventorySlots[i].GetComponent<InventorySlot>();
            if (restrictByItemType && inventory.InventoryItems[i].Item != null &&inventory.InventoryItems[i].Item.ItemType != itemTypeRestriction)
            {
                itemData = new InventoryItemData(null, 0, -1);
                slot.Disable();
            }
            slot.Setup(itemData,i,this);
        }
    }
    
    #if UNITY_EDITOR

    [Button]
    private void ClearDisplays()
    {
        this.transform.DestroyChildrenInEditor();
    }
    
    #endif

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
