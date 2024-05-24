using System;
using System.Collections.Generic;
using System.Linq;
using ParentHouse.UI;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ParentHouse {
    public class InventoryWindowDisplay : WindowDisplay, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler {
        public static InventoryWindowDisplay HighlightedInventoryWindowDisplay;
        public static readonly List<InventoryWindowDisplay> OpenInventoryWindows = new();
        public Inventory Inventory;
        [SerializeField] private Transform InventoryContainer;
        [SerializeField] private GameObject InventorySlotPrefab;

        [field: SerializeField]
        [field: FoldoutGroup("Toggles")]
        public bool IsLocked { get; private set; }

        [field: SerializeField]
        [field: FoldoutGroup("Toggles")]
        public bool RemoveOnly { get; private set; }

        [field: SerializeField]
        [field: FoldoutGroup("Toggles")]
        public bool RestrictByItemType { get; private set; }

        [field: SerializeField]
        [field: FoldoutGroup("Toggles")]
        public bool IgnoreScrollInput { get; private set; }

        [ShowIf("RestrictByItemType")] public List<ItemTypeListItem> AllowedTypes = new();
        private readonly List<GameObject> inventorySlots = new();

        private bool flaggedToUpdate;

        public void OnPointerEnter(PointerEventData eventData) {
            if ((HighlightedInventoryWindowDisplay is null || (HighlightedInventoryWindowDisplay != this && !IsLocked)) &&
                !RemoveOnly)
                HighlightedInventoryWindowDisplay = this;
        }

        public void OnPointerExit(PointerEventData eventData) {
            HighlightedInventoryWindowDisplay = null;
        }

        public void OnPointerMove(PointerEventData eventData) {
        }

        public override void Show() {
            base.Show();

            UpdateInventoryDisplay();
            if (!IgnoreScrollInput)
                OpenInventoryWindows.Add(this);
        }

        public override void Hide() {
            base.Hide();
            if (!IgnoreScrollInput && OpenInventoryWindows.Contains(this))
                OpenInventoryWindows.Remove(this);
        }

        private void PopulateDisplay() {
            if (inventorySlots.Count <= Inventory.InventorySlotLimit)
                for (var i = inventorySlots.Count; i < Inventory.InventorySlotLimit; i++) {
                    var obj = Instantiate(InventorySlotPrefab, InventoryContainer);
                    inventorySlots.Add(obj);
                }
        }

        [Button]
        public void UpdateInventoryDisplay() {
            PopulateDisplay();
            for (var i = 0; i < Inventory.InventorySlotLimit; i++) {
                var itemData = Inventory.InventoryItems[i];
                var slot = inventorySlots[i].GetComponent<InventorySlot>();

                var hasAllowedItemType =
                    AllowedTypes.Any(o => itemData.Item == null || o.Type == itemData.Item.ItemType);

                if ((RestrictByItemType && !hasAllowedItemType) || Inventory.InventoryItems[i].Item == null) {
                    itemData = new InventoryItemData(null, 0, -1);
                    slot.Disable();
                }

                slot.AssignItemData(itemData, i, this);
            }
        }

#if UNITY_EDITOR

        [Button]
        private void ClearDisplays() {
            InventoryContainer.DestroyChildrenInEditor();
            inventorySlots.Clear();
        }

#endif
    }

    [Serializable]
    public class ItemTypeListItem {
        public ItemType Type;
    }
}