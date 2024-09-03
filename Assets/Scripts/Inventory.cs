using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FishNet;
using FishNet.Object;
using I302.Manu;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public ItemLookupTable lookupTable;
    public List<InventoryItemData> InventoryItems = new();

    public static Inventory Singleton;

    private void OnEnable() {
        // InstanceFinder.ClientManager.On += LoadInventory;
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this);
            return;
        }
    }

    private void OnDestroy() {
        // InstanceFinder.ClientManager.OnAuthenticated -= LoadInventory;
    }

    public void SetInventory(InventoryMessage incomingData, string playerId) {
        if (CurrentPlayerInfo.Data.UniqueId != playerId) return;
        print($"Trying to set inventory: {incomingData.itemNameData.Count}");
        InventoryItems.Clear();
        for (var i = 0; i < incomingData.itemNameData.Count; i++) {
            var item = lookupTable.GetItem(incomingData.itemNameData[i]);
            InventoryItems.Add(new InventoryItemData(item, incomingData.itemQuantityData[i]));
        }
    }

    public void LoadInventory() {
        var localConnection = InstanceFinder.ClientManager.Connection;
        var playerId = CurrentPlayerInfo.Data.UniqueId;
        ServerDataManager.Singleton.TryLoadPlayerInventory(localConnection, playerId);
        print("Trying to load inventory");
    }

    public void SaveInventory() {
        var playerId = CurrentPlayerInfo.Data.UniqueId;
        var saveData = new InventorySaveData(InventoryItems);
        ServerDataManager.Singleton.TrySavePlayerInventory(saveData.InventorySaveDataItems,saveData.InventorySaveDataQuantities,playerId);
    }

    public static void AddItem(Item item, int amount = 1) {
        var existingStack = Singleton.InventoryItems.FirstOrDefault(o => o.Item = item);
        if (existingStack != null) {
            existingStack.Quantity += amount;
        }
        else Singleton.InventoryItems.Add(new InventoryItemData(item, amount));
        Singleton.SaveInventory();
    }

    public static void RemoveItem(Item item, int amount = 1) {
    }
}

[Serializable]
public class
    InventoryItemData {
    public Item Item;
    public int Quantity;

    public InventoryItemData(Item item, int quantity) {
        Item = item;
        Quantity = quantity;
    }
#if UNITY_EDITOR
    [Button]
    public void ClearItemData() {
        Item = null;
        Quantity = 0;
    }
#endif
}


public class InventorySaveData : SaveData {
    public string[] InventorySaveDataItems;
    public int[] InventorySaveDataQuantities;

    // public InventorySaveData(string[] inventorySaveDataItems, int[] inventorySaveDataQuantities) {
    //     InventorySaveDataItems = new string[inventorySaveDataItems.Length];
    //     InventorySaveDataQuantities = new int[inventorySaveDataQuantities.Length];
    //     for (var i = 0; i < inventoryItems.Count; i++) {
    //         InventorySaveDataItems[i] = inventoryItems[i].Item.Name;
    //         InventorySaveDataQuantities[i] = inventoryItems[i].Quantity;
    //     }
    // }

    public InventorySaveData() {
        
    }
    
    public InventorySaveData(List<InventoryItemData> inventoryItems) {
        InventorySaveDataItems = new string[inventoryItems.Count];
        InventorySaveDataQuantities = new int[inventoryItems.Count];
        for (var i = 0; i < inventoryItems.Count; i++) {
            InventorySaveDataItems[i] = inventoryItems[i].Item.Name;
            InventorySaveDataQuantities[i] = inventoryItems[i].Quantity;
        }
    }
}