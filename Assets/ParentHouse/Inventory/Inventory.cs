using System;
using ParentHouse.Utils;
using UnityEngine;

namespace ParentHouse {
    /// <summary>
    /// Inventories are generated on owners and should use some unique file path to save and load from them.
    /// </summary>
    [Serializable]
    public class Inventory {
        
        public Inventory(int inventorySize) {
            InventorySlotLimit = inventorySize;
            LoadInventory();
        }

        private readonly int InventorySlotLimit;

        private InventoryItemData[] InventoryItems;

        #region Inventory Interaction

        public InventoryItemData GetItemDataAt(int index) {
            return InventoryItems[index];
        }

        public bool HasItem(Item itemQuery, int quantity = 1) {
            var availableQuantity = 0;
            for (var i = 0; i < InventoryItems.Length; i++) {
                if (itemQuery != InventoryItems[i].Item) continue;
                availableQuantity += InventoryItems[i].Quantity;
            }

            return availableQuantity >= quantity;
        }

        public int HasItemAt(Item itemQuery) {
            var foundItemIndex = -1;
            for (var i = 0; i < InventoryItems.Length; i++)
                if (InventoryItems[i].Item == itemQuery)
                    foundItemIndex = i;
            return foundItemIndex;
        }

        public int AddItem(Item incomingItem, int quantity = 1) {
            var remaining = quantity;

            foreach (var existingItemData in InventoryItems) {
                if (remaining <= 0) break;
                if (incomingItem != existingItemData.Item ||
                    existingItemData.Item == null) continue;

                if (existingItemData.Quantity == existingItemData.Item.StackLimit) continue;

                for (var r = remaining; r > 0; r--) {
                    if (remaining <= 0 || existingItemData.Quantity >= existingItemData.Item.StackLimit) continue;
                    existingItemData.Quantity++;
                    remaining--;
                }
            }

            foreach (var potentialEmptySlot in InventoryItems) {
                if (remaining <= 0) break;
                if (potentialEmptySlot.Item != null) continue;
                potentialEmptySlot.Item = incomingItem;
                potentialEmptySlot.Quantity = 0;
                for (var r = remaining; r > 0; r--) {
                    if (remaining <= 0 || potentialEmptySlot.Quantity >= potentialEmptySlot.Item.StackLimit) continue;
                    potentialEmptySlot.Quantity++;
                    remaining--;
                }
            }

            SaveInventory();
            return remaining;
        }

        public int AddItemAt(int targetIndex, int quantity = 1, Item incomingItem = null) {
            var remaining = quantity;
            if (remaining == 0) return 0;

            if (InventoryItems[targetIndex].Item == null && incomingItem != null) {
                InventoryItems[targetIndex].Item = incomingItem;
                InventoryItems[targetIndex].Quantity = 0;
            }

            for (var r = remaining; r > 0; r--) {
                if (remaining <= 0 ||
                    InventoryItems[targetIndex].Quantity >= InventoryItems[targetIndex].Item.StackLimit) break;
                InventoryItems[targetIndex].Quantity++;
                remaining--;
            }

            SaveInventory();
            return remaining;
        }

        public int RemoveItem(Item incomingItem, int quantity = 1) {
            var remaining = quantity;

            for (var i = 0; i < InventoryItems.Length; i++) {
                var existingItemData = InventoryItems[i];
                if (remaining <= 0) break;
                if (incomingItem != existingItemData.Item) continue;

                if (existingItemData.Quantity <= 0) continue;

                for (var r = remaining; r > 0; r--) {
                    if (remaining <= 0 || existingItemData.Quantity <= 0) continue;
                    existingItemData.Quantity--;
                    remaining--;
                }
            }

            SaveInventory();
            return remaining;
        }

        public void RemoveItemAt(int itemIndex, int quantity = 1) {
            InventoryItems[itemIndex].Quantity -= quantity;
            if (InventoryItems[itemIndex].Quantity <= 0) {
                InventoryItems[itemIndex].Item = null;
                InventoryItems[itemIndex].Quantity = 0;
            }

            SaveInventory();
        }

        public void SwapItemToOtherInventory(int item1, int item2, Inventory otherInventory) {
            var itemData1 = InventoryItems[item1];
            var itemData2 = otherInventory.InventoryItems[item2];
            InventoryItems[item1] = itemData2;
            otherInventory.InventoryItems[item2] = itemData1;
            SaveInventory();
        }

        public void SortInventoryByEmptySlots() {
            for (var i = 0; i < InventorySlotLimit; i++) {
                var targetItem = InventoryItems[i].Item;
                if (targetItem == null) continue;
                for (var j = 0; j < InventorySlotLimit; j++) {
                    if (InventoryItems[j].Item != null) continue;
                    SwapItemToOtherInventory(i, j, this);
                }
            }

            SaveInventory();
        }

        #endregion

        #region Inventory Integrity

        private void VerifyInventory() {
            foreach (var t in InventoryItems) {
                if (t.Quantity > 0) continue;
                t.Item = null;
                t.Quantity = 0;
            }
        }

        private void SaveInventory() {
            VerifyInventory();
            GameEvents.OnMoveOrAddItem.Invoke();
            // SaveLoad.SaveInventory(new InventorySaveData(this));
        }

        private void LoadInventory() {
            // InventoryItems = new InventoryItemData[InventorySlotLimit];
            // InventorySaveData saveData = SaveLoad.LoadInventory(Name);
            //
            // if (saveData is null)
            // {
            //     return;
            // }
            //
            // for (int i = 0; i < saveData.InventorySaveDataItems.Length; i++)
            // {
            //     Item loadedItem = lookupTable.GetItem(saveData.InventorySaveDataItems[i]);
            //     if (loadedItem == null)
            //     {
            //         InventoryItems[i] = new InventoryItemData(null, 0,i);
            //         continue;
            //     }
            //     int loadedQuantity = saveData.InventorySaveDataQuantities[i];
            //     InventoryItems[i] = new InventoryItemData(loadedItem,loadedQuantity,i);
            // }
            // VerifyInventory();
        }

        private void ClearInventory() {
            InventoryItems = new InventoryItemData[InventorySlotLimit];
            for (var i = 0; i < InventoryItems.Length; i++) InventoryItems[i] = new InventoryItemData(null, 0, i);
        }

        #endregion
    }

    [Serializable]
    public class InventoryItemData {
        public Item Item;
        public int Quantity;
        public int Index;

        public InventoryItemData(Item item, int quantity, int index) {
            Item = item;
            Quantity = quantity;
            Index = index;
        }

        public void ClearItemData() {
            Item = null;
            Quantity = 0;
            Index = -1;
        }
    }

    [Serializable]
    public class InventorySaveData {
        [SerializeReference] public string[] InventorySaveDataItems;
        [SerializeReference] public int[] InventorySaveDataQuantities;

        public InventorySaveData(Inventory inventory, int length) {
            InventorySaveDataItems = new string[length];
            InventorySaveDataQuantities = new int[length];

            for (var i = 0; i < length; i++) {
                if (inventory.GetItemDataAt(i).Item == null) {
                    InventorySaveDataItems[i] = string.Empty;
                    InventorySaveDataQuantities[i] = 0;
                    continue;
                }

                InventorySaveDataItems[i] = inventory.GetItemDataAt(i).Item.ItemName;
                InventorySaveDataQuantities[i] = inventory.GetItemDataAt(i).Quantity;
            }
        }
    }
}