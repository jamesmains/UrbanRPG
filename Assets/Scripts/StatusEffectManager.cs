using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] private Inventory playerEffectsInventory;
    [SerializeField] private string windowName;
    
    private void OnEnable()
    {
        WindowUtility.OnOpenWindow.Raise(windowName); // Todo: Find more appropriate area to call hud window functions
    }

    private void Update()
    {
        for (var index = 0; index < playerEffectsInventory.InventoryItems.Length; index++)
        {
            var itemData = playerEffectsInventory.InventoryItems[index];
            if (itemData.Item == null) continue;
            foreach (var effect in itemData.Item.ItemEffects)
            {
                if (effect is not ItemStatusEffect statusEffect) continue;
                if (statusEffect.OnTick() || itemData.Quantity == 0)
                {
                    playerEffectsInventory.TryRemoveItemAt(index, 1);
                    playerEffectsInventory.SortIventoryByEmptySlots();
                }
            }
        }
    }
}
