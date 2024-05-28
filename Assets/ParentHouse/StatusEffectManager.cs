using UnityEngine;

namespace ParentHouse {
    public class StatusEffectManager : MonoBehaviour {
        [SerializeField] private Inventory playerEffectsInventory;
        [SerializeField] private string windowName;

        private void Update() {
            // for (var index = 0; index < playerEffectsInventory.InventoryItems.Length; index++) {
            //     var itemData = playerEffectsInventory.InventoryItems[index];
            //     if (itemData.Item == null) continue;
            //     foreach (var effect in itemData.Item.ItemBehaviors) {
            //         //if (effect is not ItemStatusEffect statusEffect) continue;
            //         // if (statusEffect.OnTick() || itemData.Quantity == 0) {
            //         //     playerEffectsInventory.TryRemoveItemAt(index);
            //         //     playerEffectsInventory.SortIventoryByEmptySlots();
            //         // }
            //     }
            // }
        }

        private void OnEnable() {
            //WindowUtility.OnOpenWindow.Raise(windowName); // Todo: Find more appropriate area to call hud window functions
        }
    }
}