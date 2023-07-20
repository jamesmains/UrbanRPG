using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] private Inventory playerEffectsInventory;

    private void Update()
    {
        for (var index = 0; index < playerEffectsInventory.InventoryItems.Length; index++)
        {
            var itemData = playerEffectsInventory.InventoryItems[index];
            if(itemData.Item == null) continue;
            foreach (var effect in itemData.Item.ItemEffects)
            {
                print(effect);
                if (effect is not ItemStatusEffect statusEffect) continue;
                if (statusEffect.OnTick())
                {
                    print("Removing effect");
                    playerEffectsInventory.TryRemoveItemAt(index,1);
                }
            }
        }
    }
}
