using System;
using System.Collections.Generic;
using ParentHouse.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ParentHouse {
    /// <summary>
    /// Handles displaying the contents of an inventory in a window display.
    /// Todo -- Add entry point to assign inventory
    /// </summary>
    public class InventoryWindowDisplay : WindowDisplay {
        public static InventoryWindowDisplay HighlightedInventoryWindowDisplay;

        [SerializeField] [FoldoutGroup("Dependencies")]
        public Inventory CurrentInventory;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private RectTransform Content;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private GameObject InventorySlotPrefab;
        
        [SerializeField] [FoldoutGroup("Dependencies")]
        private ScrollRect ScrollView;

        [SerializeField] [FoldoutGroup("Settings")]
        public bool IsLocked;

        [SerializeField] [FoldoutGroup("Settings")]
        public bool RemoveOnly;

        [SerializeField] [FoldoutGroup("Settings")]
        public bool RestrictByItemType;

        [SerializeField] [FoldoutGroup("Settings")]
        public bool IgnoreScrollInput;

        [SerializeField] [FoldoutGroup("Settings")] [ShowIf("RestrictByItemType")]
        public List<ItemTypeListItem> AllowedTypes = new();
        
        private void PopulateDisplay() {
            // todo -- Population
        }

        [Button]
        public void UpdateInventoryDisplay() {
            // todo -- Update based on inventory change events
        }

        public void OnNavigate(InputAction.CallbackContext context) {
            print(context.ReadValue<Vector2>());
            Content.position += (Vector3)context.ReadValue<Vector2>() * 100;
        }
    }

    [Serializable]
    public class ItemTypeListItem {
        public ItemType Type;
    }
}