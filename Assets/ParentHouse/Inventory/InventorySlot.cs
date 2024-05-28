using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ParentHouse {
    /// <summary>
    /// Todo -- need to remove responsibility of dragged item out of class
    /// </summary>
    public class InventorySlot : MonoBehaviour {
        private static InventorySlot movingItem;
        private static InventorySlot highlightedInventorySlot;

        [SerializeField] [FoldoutGroup("Settings")]
        private Color NormalColor;

        [SerializeField] [FoldoutGroup("Settings")]
        private Color HighlightedColor;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private Image SlotFrame;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private Image SlotIcon;

        [SerializeField] [FoldoutGroup("Dependencies")]
        private TextMeshProUGUI SlotItemCountText;

        [SerializeField] [FoldoutGroup("Status")]
        private InventoryWindowDisplay ParentWindow; // Todo - reassess the use of this

        [SerializeField] [FoldoutGroup("Status")]
        private bool MouseDown;

        [SerializeField] [FoldoutGroup("Status")]
        private bool WaitingForDrag;

        [SerializeField] [FoldoutGroup("Status")]
        private bool Splitting;

        [SerializeField] [FoldoutGroup("Status")]
        private bool TryingToSplit;

        [SerializeField] [FoldoutGroup("Status")]
        private bool TryingToConsume;

        private void Update() {
        }


        // todo - rework so that it just gets the data it needs
        public void AssignItemData(int itemIndex) {
        }

        private void ToggleHighlight(bool state) {
            SlotFrame.color = state ? HighlightedColor : NormalColor;
        }
    }
}