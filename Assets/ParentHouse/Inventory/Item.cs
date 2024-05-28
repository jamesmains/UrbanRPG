using System;
using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    public enum ItemType {
        Junk,
        Item,
        Tool,
        Quest,
        Ride
    }
    
    [CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
    public class Item : SerializedScriptableObject {

        public Item() {
            if (string.IsNullOrEmpty(ItemId))
                ItemId = Guid.NewGuid().ToString();
        }
        
        [SerializeField] [FoldoutGroup("Settings")] [field: PreviewField]
        public Sprite ItemIcon;

        [SerializeField] [FoldoutGroup("Settings")]
        public string ItemName;

        [SerializeField] [FoldoutGroup("Settings")] [field: TextArea]
        public string ItemDescription;

        [SerializeField] [FoldoutGroup("Settings")]
        public ItemType ItemType;

        [SerializeField] [FoldoutGroup("Settings")] [Tooltip("How much it would cost the player to buy from a vendor")]
        public float BuyValue;

        [SerializeField] [FoldoutGroup("Settings")] [Tooltip("How much it can be sold by the player to a vendor")]
        public float SellValue;

        [SerializeField] [FoldoutGroup("Settings")]
        public bool IsConsumable;

        [SerializeField] [FoldoutGroup("Settings")]
        public int StackLimit = 999;

        [SerializeField] [FoldoutGroup("Settings")]
        public List<Behavior> ItemBehaviors = new();

        [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
        public string ItemId;
        
        [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
        public bool IsIdentified;

        [SerializeField] [FoldoutGroup("Events")]
        public UnityEvent OnPickupItem = new();
    }

    
}