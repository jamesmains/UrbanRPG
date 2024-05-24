using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ParentHouse {
    public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
        IPointerExitHandler {
        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private static InventorySlot movingItem;

        private static InventorySlot highlightedInventorySlot;

        [field: SerializeField] [field: FoldoutGroup("Looks")]
        private Color normalColor;

        [field: SerializeField] [field: FoldoutGroup("Looks")]
        private Color highlightedColor;

        [field: SerializeField] [field: FoldoutGroup("Hooks")]
        private Vector2 playerPositionVariable; // todo - why the fuck does the inventory slot need to know this?

        [field: SerializeField] [field: FoldoutGroup("Hooks")]
        private GameObject pickupItemObject;

        [field: SerializeField] [field: FoldoutGroup("Hooks")]
        private Image frameDisplay;

        [field: SerializeField] [field: FoldoutGroup("Hooks")]
        private Image iconDisplay;

        [field: SerializeField] [field: FoldoutGroup("Hooks")]
        private TextMeshProUGUI countDisplayText;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private InventoryWindowDisplay parentInventoryWindowDisplay;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private bool mouseDown;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private bool waitingForDrag;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private bool splitting;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private bool tryingToSplit;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private bool tryingToConsume;

        [field: SerializeField] [field: FoldoutGroup("Debug")] [field: ReadOnly]
        private Inventory thisInventory;

        [field: SerializeField] public InventoryItemData storedItemData = new(null, 0, -1);

        private void Update() {
            if (parentInventoryWindowDisplay == null || parentInventoryWindowDisplay.IsLocked) return;
            if (waitingForDrag && mouseDown && Vector2.Distance(Input.mousePosition, transform.position) > 85f) {
                waitingForDrag = false;
                if (movingItem == null) Drag();
            }
            else if (!mouseDown) {
                waitingForDrag = false;
            }
        }

        private void OnEnable() {
            // GameEvents.OnAltMouseButtonUp += delegate
            // {
            //     if (Global.PlayerLock > 0) return;
            //     if (splitting)
            //     {
            //         tryingToSplit = false;
            //         splitting = false;
            //         return;
            //     }
            //     tryingToSplit = false;
            //     splitting = false;
            //     
            //         TryConsumeItem();
            // };
            // GameEvents.OnAltMouseButtonDown += delegate
            // {
            //     if (highlightedInventorySlot != this) return;
            //     tryingToSplit = true;
            //     tryingToConsume = true;
            // };
        }

        private void OnDisable() {
        }

        public void OnPointerDown(PointerEventData eventData) {
            mouseDown = true;
            waitingForDrag = true;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if ((highlightedInventorySlot is null || highlightedInventorySlot != this) &&
                !parentInventoryWindowDisplay.IsLocked) {
                if (highlightedInventorySlot is not null) highlightedInventorySlot.ToggleHighlight(false);
                highlightedInventorySlot = this;
                ToggleHighlight(true);
            }

            if (storedItemData.Item != null) {
                var n = storedItemData.Item.Name;
                var t = storedItemData.Item.ItemType.ToString();
                var d = storedItemData.Item.Description;
                var tooltipMessage = $"{n}\n{t}\n{d}";
                GameEvents.OnShowTooltip.Invoke(tooltipMessage);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (highlightedInventorySlot != null) {
                highlightedInventorySlot.ToggleHighlight(false);
                highlightedInventorySlot = null;
            }

            GameEvents.OnMouseExitInventorySlot.Invoke();
            GameEvents.OnHideTooltip.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData) {
            mouseDown = false;
            TryReleaseItem();
        }


        public void AssignItemData(InventoryItemData inventoryItemData, int index, InventoryWindowDisplay origin,
            string overrideText = "") {
            if (inventoryItemData.Item is Gear item && inventoryItemData != storedItemData && origin.RestrictByItemType)
                if (!HasAllowedItemType(origin, inventoryItemData.Item))
                    foreach (var effect in item.GearEffects)
                        effect.OnEquip();

            storedItemData = inventoryItemData;
            storedItemData.Index = index;
            parentInventoryWindowDisplay = origin;
            thisInventory = origin.Inventory;

            if (origin.Inventory.InventoryItems[index].Item == null || storedItemData.Item == null) {
                Disable();
                return;
            }

            iconDisplay.enabled = true;
            iconDisplay.sprite = origin.Inventory.InventoryItems[storedItemData.Index].Item.Sprite;

            countDisplayText.enabled = true;
            countDisplayText.text = origin.Inventory.InventoryItems[storedItemData.Index].Quantity.ToString();

            if (origin.Inventory.InventoryItems[storedItemData.Index].Quantity == 1) countDisplayText.enabled = false;

            if (string.IsNullOrEmpty(overrideText)) return;
            countDisplayText.enabled = true;
            countDisplayText.text = overrideText;
        }

        public void Disable() {
            iconDisplay.enabled = false;
            countDisplayText.enabled = false;
        }

        private void Drag() {
            if (parentInventoryWindowDisplay.Inventory.InventoryItems[storedItemData.Index].Quantity <= 0 ||
                storedItemData.Item == null) return;
            if (tryingToSplit) splitting = true;
            tryingToConsume = false;
            movingItem = this;
            GameEvents.OnItemMove.Invoke(parentInventoryWindowDisplay.Inventory.InventoryItems[storedItemData.Index]
                .Item);
            iconDisplay.color = new Color(1, 1, 1, 0.5f);
        }

        private void DropItem(int q = -1) {
            var itemData = parentInventoryWindowDisplay.Inventory.InventoryItems[storedItemData.Index];
            var quantity = q < 0 ? itemData.Quantity : q;
            quantity = tryingToSplit ? quantity / 2 : quantity;
            if (quantity == 0) return;

            // Why is this responsible for this? Needs pooler
            Vector3 spawnPos = playerPositionVariable;
            var pickup = Instantiate(pickupItemObject, spawnPos, Quaternion.identity).GetComponent<Pickup>();

            pickup.Setup(itemData.Item, quantity);
            thisInventory.TryRemoveItemAt(storedItemData.Index, quantity);
            if (thisInventory.InventoryItems[storedItemData.Index].Quantity <= 0)
                storedItemData = null;
        }

        private void TryReleaseItem() {
            if (movingItem == null) return;
            if (this != highlightedInventorySlot && highlightedInventorySlot is not null) {
                TryMoveItemToSlot();
            }
            else if (InventoryWindowDisplay.HighlightedInventoryWindowDisplay != parentInventoryWindowDisplay &&
                     InventoryWindowDisplay.HighlightedInventoryWindowDisplay != null) {
                var quantity = tryingToSplit ? storedItemData.Quantity / 2 : storedItemData.Quantity;
                TryMoveItemToInventoryWindow(InventoryWindowDisplay.HighlightedInventoryWindowDisplay, quantity);
            }
            else if (!Global.IsMouseOverUI) {
                DropItem();
            }

            movingItem = null;
            iconDisplay.color = new Color(1, 1, 1, 1);
            GameEvents.OnItemRelease.Invoke();
            GameEvents.OnMoveOrAddItem.Invoke();
        }

        private void TryConsumeItem() {
            if (highlightedInventorySlot != this || storedItemData.Item == null || !tryingToConsume) return;
            if (storedItemData.Item.IsConsumable) {
                foreach (var itemEffect in storedItemData.Item.ItemEffects) itemEffect.OnConsume();
                thisInventory.TryRemoveItemAt(storedItemData.Index);
            }
        }

        private void TryMoveItemToSlot() {
            var his = highlightedInventorySlot;
            var hisWindowPanel = his.parentInventoryWindowDisplay;

            if (hisWindowPanel.RemoveOnly) return;
            if (hisWindowPanel.RestrictByItemType && !HasAllowedItemType(hisWindowPanel)) return;
            if (storedItemData.Item == his.storedItemData.Item || his.storedItemData.Item == null)
                AddItemToExistingStack(hisWindowPanel, his.storedItemData.Index);
            else
                SwapItemStacks(hisWindowPanel, his.storedItemData.Index);
        }

        private void AddItemToExistingStack(InventoryWindowDisplay hisWindowDisplay, int hisIndex) {
            var thisItemData = thisInventory.InventoryItems[storedItemData.Index];
            var quantity = thisItemData.Quantity;
            quantity = tryingToSplit ? quantity / 2 : quantity;
            var overflow = hisWindowDisplay.Inventory.TryAddItemAt(hisIndex, quantity, storedItemData.Item);
            thisInventory.TryRemoveItemAt(storedItemData.Index, quantity - overflow);
        }

        private void SwapItemStacks(InventoryWindowDisplay hisWindowDisplay, int hisIndex) {
            if (hisWindowDisplay.RestrictByItemType && !HasAllowedItemType(hisWindowDisplay)) return;
            thisInventory.TrySwapItem(storedItemData.Index, hisIndex, hisWindowDisplay.Inventory);
        }

        private void TryMoveItemToOtherOpenWindow(float movement) {
            if (movement == 0 || storedItemData.Item == null || movingItem != null) return;
            if (InventoryWindowDisplay.OpenInventoryWindows.Count <= 1 || highlightedInventorySlot != this) return;

            var index = InventoryWindowDisplay.OpenInventoryWindows.IndexOf(parentInventoryWindowDisplay);
            index += (int) Mathf.Abs(movement) * movement > 0 ? 1 : -1;
            if (index >= InventoryWindowDisplay.OpenInventoryWindows.Count) index = 0;
            else if (index < 0) index = InventoryWindowDisplay.OpenInventoryWindows.Count - 1;

            if (movement > 0)
                TryMoveItemToInventoryWindow(InventoryWindowDisplay.OpenInventoryWindows[index], 1);
            else if (movement < 1)
                TryGetItemFromInventoryWindow(InventoryWindowDisplay.OpenInventoryWindows[index], 1);
        }

        private void TryMoveItemToInventoryWindow(InventoryWindowDisplay targetWindowDisplay, int amount) {
            if ((targetWindowDisplay.RestrictByItemType && !HasAllowedItemType(targetWindowDisplay)) ||
                targetWindowDisplay.RemoveOnly) return;
            var i = parentInventoryWindowDisplay.Inventory.InventoryItems[storedItemData.Index];
            var overflow = targetWindowDisplay.Inventory.TryAddItem(i.Item, amount);
            parentInventoryWindowDisplay.Inventory.TryRemoveItemAt(storedItemData.Index, amount - overflow);
        }

        private void TryGetItemFromInventoryWindow(InventoryWindowDisplay targetWindowDisplay, int amount) {
            if (targetWindowDisplay.RestrictByItemType && !HasAllowedItemType(targetWindowDisplay)) return;
            var i = targetWindowDisplay.Inventory.HasItemAt(storedItemData.Item);
            if (i < 0) return;
            var overflow =
                parentInventoryWindowDisplay.Inventory.TryAddItemAt(storedItemData.Index, amount, storedItemData.Item);
            targetWindowDisplay.Inventory.TryRemoveItemAt(i, amount - overflow);
        }

        private bool HasAllowedItemType(InventoryWindowDisplay targetWindowDisplay) {
            if (storedItemData.Item == null) return false;
            return targetWindowDisplay.AllowedTypes.Any(o => o.Type == storedItemData.Item.ItemType);
        }

        private bool HasAllowedItemType(InventoryWindowDisplay targetWindowDisplay, Item targetItem) {
            if (targetItem == null) return false;
            return targetWindowDisplay.AllowedTypes.Any(o => o.Type == targetItem.ItemType);
        }

        private void ToggleHighlight(bool state) {
            frameDisplay.color = state ? highlightedColor : normalColor;
        }
    }
}