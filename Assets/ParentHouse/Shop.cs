using System;
using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Shop", menuName = "Signatures/Shop")]
    public class Shop : SerializedScriptableObject
    {
        // TODO: Add save / load function to capture how many items are left between play sessions
    
        [SerializeField, FoldoutGroup("Details")] private string storeName;
        public Inventory targetInventory; // Where the purchases go -- most go to pockets but some might go to mail
        [FoldoutGroup("Data")] public List<ShopItem> storeItems;

        public void OpenShop()
        {
            GameEvents.OnOpenShop.Invoke(this);
        }

        public void RestockShop()
        {
            foreach (var storeItem in storeItems)
            {
                storeItem.currentQuantity = storeItem.maxQuantity;
            }
        }
    }

    [Serializable]
    public class ShopItem
    {
        public Item item;
        public int currentQuantity;
        public int maxQuantity;
    }
}