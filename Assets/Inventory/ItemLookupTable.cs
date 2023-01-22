using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace I302.Manu
{
    [CreateAssetMenu(fileName = "ItemLookupTable", menuName = "Scriptable Objects/ItemsAndInventory/LookupTable")]
    public class ItemLookupTable : SerializedScriptableObject
    {
        [field: SerializeField]
        public Dictionary<string, Item> ItemCollection { get; private set; } = new Dictionary<string, Item>();

        public Item GetItem(string itemName)
        {
            return ItemCollection[itemName];
        }
        
#if UNITY_EDITOR
        [FoldoutGroup("Automation")][Button]
        public void PopulateItemCollection()
        {
            List<Item> itemList = AssetManagementUtil.GetAllScriptableObjectInstances<Item>();

            foreach (Item item in itemList)
            {
                ItemCollection.Add(item.Name, item);
            }
        }
#endif
    }
}
