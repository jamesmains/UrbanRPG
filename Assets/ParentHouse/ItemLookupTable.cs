using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse
{
    [CreateAssetMenu(fileName = "ItemLookupTable", menuName = "Unsorted/LookupTable")]
    public class ItemLookupTable : SerializedScriptableObject
    {
        [field: SerializeField]
        public Dictionary<string, Item> ItemCollection { get; private set; } = new Dictionary<string, Item>();

        public Item GetItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName)) return null;
            return ItemCollection[itemName];
        }
        
#if UNITY_EDITOR
        [FoldoutGroup("Automation")][Button]
        public void PopulateItemCollection()
        {
            ItemCollection.Clear();
            List<Item> itemList = AssetManagementUtil.GetAllScriptableObjectInstances<Item>();

            foreach (Item item in itemList)
            {
                ItemCollection.Add(item.Name, item);
            }
        }
#endif
    }
}
