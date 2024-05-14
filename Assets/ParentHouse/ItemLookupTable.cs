using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "ItemLookupTable", menuName = "Unsorted/LookupTable")]
    public class ItemLookupTable : SerializedScriptableObject {
        [field: SerializeField] public Dictionary<string, Item> ItemCollection { get; private set; } = new();

        public Item GetItem(string itemName) {
            if (string.IsNullOrEmpty(itemName)) return null;
            return ItemCollection[itemName];
        }

#if UNITY_EDITOR
        [FoldoutGroup("Automation")]
        [Button]
        public void PopulateItemCollection() {
            ItemCollection.Clear();
            var itemList = AssetManagementUtil.GetAllScriptableObjectInstances<Item>();

            foreach (var item in itemList) ItemCollection.Add(item.Name, item);
        }
#endif
    }
}