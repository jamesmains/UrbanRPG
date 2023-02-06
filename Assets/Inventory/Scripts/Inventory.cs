using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace I302.Manu
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/ItemsAndInventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [SerializeReference] public Dictionary<Item, int> ItemList = new();
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
            foreach (Item storedItem in ItemList.Keys)
            {
                if (storedItem == incomingItem)
                {
                    int combinedTotal = ItemList[storedItem] + value;
                    if (combinedTotal > storedItem.StackLimit)
                    {
                        int overflow = combinedTotal - storedItem.StackLimit;
                        ItemList[storedItem] = storedItem.StackLimit;
                        return overflow;
                    }

                    ItemList[storedItem] += value;
                    SaveInventory();
                    return 0;
                }
            }

            if (ItemList.Count >= InventorySlotLimit.Value)
                return value;

            ItemList.Add(incomingItem,value);
            SaveInventory();
            return 0;
        }

        public void TryUseItem(Item neededItem, int amount = 1)
        {
            ItemList[neededItem] -= amount;
            if (ItemList[neededItem] <= 0)
                RemoveItem(neededItem);
        }

        private void RemoveItem(Item removedItem)
        {
            ItemList.Remove(removedItem);
            SaveInventory();
        }

        public bool HasItem(Item itemQuery)
        {
            var i = ItemList.Keys.ToList().Find(o => o == itemQuery);
            return i != null && ItemList[i] > 0;
        }

        public List<Item> GetListOfItemsByType(ItemType queryType)
        {
            return ItemList.Keys.ToList().FindAll(o => o.ItemType == queryType);
        }

        [FoldoutGroup("Saving and Loading")][Button]
        public void SaveAllItems()
        {
            foreach (Item item in ItemList.Keys)
            {
                if (ItemList[item] <= 0)
                    ItemList.Remove(item);
            }
        }

        [FoldoutGroup("Saving and Loading")]
        [Button]
        public void LoadAllItems()
        {
            foreach (Item item in ItemList.Keys)
            {
                if (ItemList[item] <= 0)
                    ItemList.Remove(item);
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
        
            foreach (string itemKey in saveData.ItemDataList.Keys)
            {
                var i = lookupTable.GetItem(itemKey);
                ItemList.Add(i,saveData.ItemDataList[i.Name]);
            }
        }
    }

    [Serializable]
    public class InventorySaveData
    {
        public string Name { get; private set; }   
        public Dictionary<string,int> ItemDataList { get; private set; } = new ();
        
        public InventorySaveData(Inventory inventory)
        {
            Name = inventory.Name;
            foreach (Item item in inventory.ItemList.Keys)
            {
                ItemDataList.Add(item.Name,inventory.ItemList[item]);
            }
        }
    }
}
