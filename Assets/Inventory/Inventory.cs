using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace I302.Manu
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/ItemsAndInventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<Item> ItemList { get; private set; }
        [field: SerializeField] public IntVariable InventorySlotLimit; 

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (Name.IsNullOrWhitespace())
            {
                Debug.LogError($"{this} inventory has no name!");
            }
        }
#endif

        public int TryAddItem(Item incomingItem, int value)
        {
            foreach (Item storedItem in ItemList)
            {
                if (storedItem == incomingItem)
                {
                    int combinedTotal = storedItem.Value + value;
                    if (combinedTotal > storedItem.StackLimit)
                    {
                        int overflow = combinedTotal - storedItem.StackLimit;
                        storedItem.Value = storedItem.StackLimit;
                        return overflow;
                    }

                    storedItem.Value += value;
                    SaveInventory();
                    return 0;
                }
            }

            if (ItemList.Count >= InventorySlotLimit.Value)
                return value;

            ItemList.Add(incomingItem);
            incomingItem.Value += value;
            SaveInventory();
            return 0;
        }

        public void TryUseItem(Item neededItem, int amount = 1)
        {
            neededItem.Value -= amount;
            if (neededItem.Value <= 0)
                RemoveItem(neededItem);
        }

        private void RemoveItem(Item removedItem)
        {
            ItemList.Remove(removedItem);
            SaveInventory();
        }

        public bool HasItem(Item itemQuery)
        {
            var i = ItemList.Find(o => o == itemQuery);
            return i != null && i.Value > 0;
        }

        public List<Item> GetListOfItemsByType(ItemType queryType)
        {
            return ItemList.FindAll(o => o.ItemType == queryType);
        }

        [FoldoutGroup("Saving and Loading")][Button]
        public void SaveAllItems()
        {
            foreach (Item item in ItemList)
            {
                if (item.Value <= 0)
                    ItemList.Remove(item);
                item.SaveItem();
            }
        }

        [FoldoutGroup("Saving and Loading")]
        [Button]
        public void LoadAllItems()
        {
            foreach (Item item in ItemList)
            {
                if (item.Value <= 0)
                    ItemList.Remove(item);
                item.LoadItem();
            }
        }

        [FoldoutGroup("Saving and Loading")][Button]
        public void SaveInventory()
        {
            SaveLoad.SaveInventory(new InventorySaveData(this));
        }

        [FoldoutGroup("Saving and Loading")][Button]
        public void LoadInventory(ItemLookupTable lookupTable)
        {
            ItemList.Clear();

            InventorySaveData saveData = SaveLoad.LoadInventory(Name);

            if (saveData is null)
            {
                return;
            }

            foreach (string itemKey in saveData.ItemNameList)
            {
                ItemList.Add(lookupTable.GetItem(itemKey));
            }
        }
    }

    [Serializable]
    public class InventorySaveData
    {
        public string Name { get; private set; }   
        public List<string> ItemNameList { get; private set; } = new List<string>();
        
        public InventorySaveData(Inventory inventory)
        {
            Name = inventory.Name;
            foreach (Item item in inventory.ItemList)
            {
                ItemNameList.Add(item.Name);
            }
        }
    }
}
