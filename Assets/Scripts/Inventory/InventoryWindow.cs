using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryWindow : Window,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    public Inventory Inventory;
    [SerializeField] private Transform InventoryContainer;
    [SerializeField] private GameObject InventorySlotPrefab;
    [field: SerializeField, FoldoutGroup("Toggles")] public bool IsLocked {get; private set;}
    [field: SerializeField, FoldoutGroup("Toggles")] public bool RemoveOnly {get; private set;}
    [field: SerializeField, FoldoutGroup("Toggles")] public bool RestrictByItemType {get; private set;}
    [field: SerializeField, FoldoutGroup("Toggles")] public bool IgnoreScrollInput {get; private set;}
    [ShowIf("RestrictByItemType")] public List<ItemTypeListItem> AllowedTypes = new();

    private bool flaggedToUpdate;
    private readonly List<GameObject> inventorySlots = new();
    
    public static InventoryWindow HighlightedInventoryWindow;
    public static readonly List<InventoryWindow> OpenInventoryWindows = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        PopulateDisplay();
        GameEvents.OnPickupItem.AddListener(UpdateInventoryDisplay);
        GameEvents.OnMoveOrAddItem.AddListener(UpdateInventoryDisplay);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnPickupItem.RemoveListener(UpdateInventoryDisplay);
        GameEvents.OnMoveOrAddItem.RemoveListener(UpdateInventoryDisplay);
    }

    public override void Show()
    {
        base.Show();
        
        UpdateInventoryDisplay();
        if(!IgnoreScrollInput)
            OpenInventoryWindows.Add(this);
    }

    public override void Hide()
    {
        base.Hide();
        if(!IgnoreScrollInput && OpenInventoryWindows.Contains(this))
            OpenInventoryWindows.Remove(this);
    }

    private void PopulateDisplay()
    {
        if (inventorySlots.Count <= Inventory.InventorySlotLimit)
        {
            for (int i = inventorySlots.Count; i < Inventory.InventorySlotLimit; i++)
            {
                var obj = Instantiate(InventorySlotPrefab, InventoryContainer);
                inventorySlots.Add(obj);
            }
        }
    }
    
    [Button]
    public void UpdateInventoryDisplay()
    {
        PopulateDisplay();
        for (int i = 0; i < Inventory.InventorySlotLimit; i++)
        {
            var itemData = Inventory.InventoryItems[i];
            var slot = inventorySlots[i].GetComponent<InventorySlot>();
            
            bool hasAllowedItemType = AllowedTypes.Any(o => itemData.Item == null || o.Type == itemData.Item.ItemType);
            
            if (RestrictByItemType && !hasAllowedItemType || Inventory.InventoryItems[i].Item == null )
            {
                itemData = new InventoryItemData(null, 0, -1);
                slot.Disable();
            }
            slot.AssignItemData(itemData,i,this);
        }
    }
    
    #if UNITY_EDITOR

    [Button]
    private void ClearDisplays()
    {
        InventoryContainer.DestroyChildrenInEditor();
        inventorySlots.Clear();
    }
    
    #endif

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((HighlightedInventoryWindow is null || HighlightedInventoryWindow != this && !IsLocked) && !RemoveOnly)
            HighlightedInventoryWindow = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightedInventoryWindow = null;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        
    }
}

[Serializable]
public class ItemTypeListItem
{
    public ItemType Type;
}