using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using UnityEngine;

public struct PlayerDataMessage : IBroadcast {
    
}

public struct InventoryMessage : IBroadcast {
    public string username;
    public List<string> itemNameData;
    public List<int> itemQuantityData;
}

public struct ChatHistory : IBroadcast {
    public string username;
    public string roomChannel;
    public List<Message> messages;
}

public class ServerDataManager : NetworkBehaviour {
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Queue<Message> ServerLoggedMessages = new();

    private const int MAX_CHAT_LOG = 5;
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
        // Inventory requests
        InstanceFinder.ClientManager.RegisterBroadcast<InventoryMessage>(OnClientInventoryRequestReceived);
        InstanceFinder.ServerManager.RegisterBroadcast<InventoryMessage>(OnServerInventoryMessageReceived);
        
        // Chat history requests
        InstanceFinder.ClientManager.RegisterBroadcast<ChatHistory>(OnClientChatHistoryRequestReceived);
        InstanceFinder.ServerManager.RegisterBroadcast<ChatHistory>(OnServerChatHistoryMessageReceived);
    }

    public void Disable() {
        // Inventory requests
        InstanceFinder.ClientManager.UnregisterBroadcast<InventoryMessage>(OnClientInventoryRequestReceived);
        InstanceFinder.ServerManager.UnregisterBroadcast<InventoryMessage>(OnServerInventoryMessageReceived);
        
        // Chat history requests
        InstanceFinder.ClientManager.UnregisterBroadcast<ChatHistory>(OnClientChatHistoryRequestReceived);
        InstanceFinder.ServerManager.UnregisterBroadcast<ChatHistory>(OnServerChatHistoryMessageReceived);
    }

    [ServerRpc]
    public void TryLoadPlayerInventory(NetworkConnection connection, string playerId) {
        if (!SaveLoad.HasPath(SaveLoad.PlayerInventoryPath(playerId))) return;
        var itemData = (InventorySaveData)SaveLoad.Load(SaveLoad.PlayerInventoryPath(playerId));
        var inventoryMessage = new InventoryMessage {
            itemNameData = itemData.InventorySaveDataItems.ToList(),
            itemQuantityData = itemData.InventorySaveDataQuantities.ToList(),
            username = playerId
        };
        InstanceFinder.ServerManager.Broadcast(connection, inventoryMessage, true);
    }

    [ServerRpc]
    public void TrySavePlayerInventory(string[] InventorySaveDataItems,int[] InventorySaveDataQuantities, string playerId) {
        print("Trying to save player inventory");
        InventorySaveData data = new InventorySaveData();
        data.InventorySaveDataItems = InventorySaveDataItems;
        data.InventorySaveDataQuantities = InventorySaveDataQuantities;
        SaveLoad.Save(data, SaveLoad.PlayerInventoryPath(playerId));
    }
    
    private void OnClientInventoryRequestReceived(InventoryMessage message, Channel channel = Channel.Reliable) {
        Inventory.Singleton.SetInventory(message,message.username);
    }
    
    private void OnServerInventoryMessageReceived(NetworkConnection networkConnection, InventoryMessage msg, Channel channel = Channel.Reliable)
    {
        InstanceFinder.ServerManager.Broadcast(msg);
    }

    [ServerRpc]
    public void TryLogMessage(Message msg) {
        ServerLoggedMessages.Enqueue(msg);
        if (ServerLoggedMessages.Count > MAX_CHAT_LOG) {
            ServerLoggedMessages.Dequeue();
        }
    }
    
    [ServerRpc]
    public void TryRequestChatHistory(NetworkConnection connection, string playerId, string roomChannel) {
        var chatHistory = new ChatHistory() {
            username = playerId,
            roomChannel = roomChannel,
            messages = ServerLoggedMessages.Where(m => m.roomChannel == roomChannel).ToList()
        };
        InstanceFinder.ServerManager.Broadcast(connection, chatHistory, true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TrySetChatHistory(string[] InventorySaveDataItems,int[] InventorySaveDataQuantities, string playerId) {
    }
    
    private void OnClientChatHistoryRequestReceived(ChatHistory chatHistory, Channel channel = Channel.Reliable) {
        Chatbox.Singleton.SetChatHistory(chatHistory,chatHistory.username);
    }
    
    private void OnServerChatHistoryMessageReceived(NetworkConnection networkConnection, ChatHistory msg, Channel channel = Channel.Reliable)
    {
        InstanceFinder.ServerManager.Broadcast(msg);
    }
    
    
}
