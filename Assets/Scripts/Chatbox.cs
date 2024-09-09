using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Todo: Replace spawning individual objects for messages with single text that appends based on filter
/// Todo: Add filters to chat messages
///     Possible filters -- Room, Friends, Trade, Events, Proximity 
/// Todo: Add global profanity filter
/// Todo: Add global spam filter
/// </summary>
public struct Message : IBroadcast {
    public string username;
    public string message;
    public string roomChannel;
}

public class Chatbox : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private TMP_InputField ChatInputField;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private ScrollRect Scroll;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private LayoutGroup Layout;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private ContentSizeFitter Fitter;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private GameObject ChatTextObject;

    [SerializeField] [FoldoutGroup("Settings")]
    private RectTransform Content;

    [SerializeField] [FoldoutGroup("Settings")]
    private string RoomChannel = ROOM_GLOBAL;

    [SerializeField] [FoldoutGroup("Debug")]
    private TextMeshProUGUI DebugCurrentChannel;

    [SerializeField] [FoldoutGroup("Debug")]
    private TextMeshProUGUI DebugCurrentRoom;


    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private string CurrentRoomName;

    public bool IsFocussedOnText() => ChatInputField.isFocused;

    public static Chatbox Singleton;

    public const string ROOM_GLOBAL = "Global";
    public const string ROOM_TRADE = "Trade";


    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
            // SetChatChannelGlobal();
        }
        else Destroy(this);
    }

    private void OnEnable() {
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageReceived);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnClientMessageRecieved);
        InstanceFinder.SceneManager.OnLoadEnd += ChangeCurrentRoomNameCache;
    }

    public void Disable() {
        InstanceFinder.SceneManager.OnLoadEnd -= ChangeCurrentRoomNameCache;
        InstanceFinder.ClientManager.UnregisterBroadcast<Message>(OnMessageReceived);
        InstanceFinder.ServerManager.UnregisterBroadcast<Message>(OnClientMessageRecieved);
    }

    private void ChangeCurrentRoomNameCache(SceneLoadEndEventArgs sceneLoadEndEventArgs) {
        if (sceneLoadEndEventArgs.LoadedScenes.Length == 0) return;

        CurrentRoomName = sceneLoadEndEventArgs.LoadedScenes[0].name; // Not EXACTLY the best way, but good enough
        DebugCurrentRoom.text = $"Room: {CurrentRoomName}";
    }

    // private void UnregisterListeners(SceneLoadEndEventArgs sceneLoadEndEventArgs) {
    //     
    // }

    public void TryFocusInputField() {
        if (IsFocussedOnText()) {
            SendMessage();
        }
        else ChatInputField.Select();
    }

    public void SetChatChannelGlobal() {
        RoomChannel = ROOM_GLOBAL;
        ChangeChannel();
    }

    public void SetChatChannelTrade() {
        RoomChannel = ROOM_TRADE;
        ChangeChannel();
    }

    public void SetChatChannelCurrentRoom() {
        RoomChannel = CurrentRoomName;
        ChangeChannel();
    }

    private void ChangeChannel() {
        DebugCurrentChannel.text = $"Channel: {RoomChannel}";
        foreach (Transform child in Content) {
            child.gameObject.SetActive(false);
        }

        var localConnection = InstanceFinder.ClientManager.Connection;
        if (!localConnection.IsActive) return;
        var playerId = CurrentPlayerInfo.Data.UniqueId;
        ServerDataManager.Singleton.TryRequestChatHistory(localConnection, playerId, RoomChannel);
    }

    public void SetChatHistory(ChatHistory chatHistory, string playerId) {
        if (CurrentPlayerInfo.Data.UniqueId != playerId) return;
        foreach (var message in chatHistory.messages) {
            DisplayMessage(message);
        }
    }

    private void SendMessage() {
        if (string.IsNullOrEmpty(ChatInputField.text)) return;
        EventSystem.current.SetSelectedGameObject(null);
        Message msg = new Message() {
            username = CurrentPlayerInfo.Data.PlayerName,
            message = ChatInputField.text,
            roomChannel = RoomChannel
        };
        ChatInputField.text = string.Empty;

        if (InstanceFinder.IsServerStarted) {
            InstanceFinder.ServerManager.Broadcast(msg);
        }
        else if (InstanceFinder.IsClientStarted) {
            InstanceFinder.ClientManager.Broadcast(msg);
        }

        ServerDataManager.Singleton.TryLogMessage(msg);
    }

    public void SendMessage(Message msg) {
        msg.roomChannel = RoomChannel;
        if (InstanceFinder.IsServerStarted) {
            InstanceFinder.ServerManager.Broadcast(msg);
        }
        else if (InstanceFinder.IsClientStarted) {
            InstanceFinder.ClientManager.Broadcast(msg);
        }

        ServerDataManager.Singleton.TryLogMessage(msg);
    }

    private void DisplayMessage(Message message) {
        var obj = Pooler.Spawn(ChatTextObject, Content, false);
        var rect = obj.transform.GetComponent<RectTransform>();
        rect.SetSiblingIndex(0);
        var textMesh = obj.GetComponent<TextMeshProUGUI>();
        textMesh.text = $"{message.username} : {message.message}";

        StartCoroutine(FixLayout());

        IEnumerator FixLayout() {
            yield return new WaitForEndOfFrame();
            Layout.CalculateLayoutInputVertical();
            Fitter.SetLayoutVertical();
            Scroll.verticalNormalizedPosition = 0;
        }
    }

    private void OnMessageReceived(Message message, Channel channel = Channel.Reliable) {
        if (RoomChannel != message.roomChannel) return;
        DisplayMessage(message);
    }

    private void OnClientMessageRecieved(NetworkConnection networkConnection, Message msg,
        Channel channel = Channel.Reliable) {
        InstanceFinder.ServerManager.Broadcast(msg);
    }
}