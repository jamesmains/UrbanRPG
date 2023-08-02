using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] private Inventory playerEffectsInventory;

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
                }
            }
        }
    }
}
