using System.Collections.Generic;
using UnityEngine;

namespace ParentHouse {
    /// <summary>
    /// This class is used for loading a player's inventory using an item's Id
    /// </summary>
    public static class ItemLookupTable{

        static ItemLookupTable() {
            Debug.Log("Creating lookup table");
            InitializeLookupTable();
        }
        
        private static Dictionary<string, Item> ItemCollection = new();

        public static Item GetItem(string itemName) {
            return string.IsNullOrEmpty(itemName) ? null : ItemCollection[itemName];
        }

        private static void InitializeLookupTable() {
            ItemCollection.Clear();
            var itemList = Resources.LoadAll<Item>("Items");
            foreach (var item in itemList) ItemCollection.Add(item.ItemId, item);
        }
    }
}