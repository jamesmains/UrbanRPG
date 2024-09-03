using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

public struct InventoryMessage : IBroadcast {
    public string username;
    public List<string> itemNameData;
    public List<int> itemQuantityData;
}

public class ServerDataManager : NetworkBehaviour {
    public static ServerDataManager Singleton;

    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
        }
        else {
            Destroy(this);
        }
    }
    
    private void OnEnable() {
        InstanceFinder.ClientManager.RegisterBroadcast<InventoryMessage>(OnMessageRecieved);
        InstanceFinder.ServerManager.RegisterBroadcast<InventoryMessage>(OnClientMessageRecieved);
    }

    public void Disable() {
        InstanceFinder.ClientManager.UnregisterBroadcast<InventoryMessage>(OnMessageRecieved);
        InstanceFinder.ServerManager.UnregisterBroadcast<InventoryMessage>(OnClientMessageRecieved);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryLoadPlayerInventory(NetworkConnection connection, string playerId) {
        //if (!IsServerStarted) return;
        if (!SaveLoad.HasPath(SaveLoad.PlayerInventoryPath(playerId))) return;
        var itemData = (InventorySaveData)SaveLoad.Load(SaveLoad.PlayerInventoryPath(playerId));
        var test = new InventoryMessage {
            itemNameData = itemData.InventorySaveDataItems.ToList(),
            itemQuantityData = itemData.InventorySaveDataQuantities.ToList(),
            username = playerId
        };
        //
        // if (InstanceFinder.IsServerStarted) {
        //     InstanceFinder.ServerManager.Broadcast(connection, test, true);
        // }
        // else if (InstanceFinder.IsClientStarted) {
        //     InstanceFinder.ClientManager.Broadcast(msg);
        // }
        print($"Trying to broadcast inventory data: {test.itemNameData.Count}");
        InstanceFinder.ServerManager.Broadcast(connection, test, true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TrySavePlayerInventory(string[] InventorySaveDataItems,int[] InventorySaveDataQuantities, string playerId) {
        
        InventorySaveData data = new InventorySaveData();
        data.InventorySaveDataItems = InventorySaveDataItems;
        data.InventorySaveDataQuantities = InventorySaveDataQuantities;
        SaveLoad.Save(data, SaveLoad.PlayerInventoryPath(playerId));
    }
    
    private void OnMessageRecieved(InventoryMessage message, Channel channel = Channel.Reliable) {
        Inventory.Singleton.SetInventory(message,message.username);
    }
    
    private void OnClientMessageRecieved(NetworkConnection networkConnection, InventoryMessage msg, Channel channel = Channel.Reliable)
    {
        InstanceFinder.ServerManager.Broadcast(msg);
    }
}
