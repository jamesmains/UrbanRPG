using System;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public struct Message : IBroadcast {
    public string username;
    public string message;
}

public class Chatbox : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")]
    private TMP_InputField ChatInputField;

    [SerializeField] [FoldoutGroup("Dependencies")]
    private GameObject ChatTextObject;

    [SerializeField] [FoldoutGroup("Settings")]
    private RectTransform Content;
    
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    public bool IsFocussedOnText ()=> ChatInputField.isFocused;


    private void OnEnable() {
        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageRecieved);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnClientMessageRecieved);
    }

    public void Disable() {
        InstanceFinder.ClientManager.UnregisterBroadcast<Message>(OnMessageRecieved);
        InstanceFinder.ServerManager.UnregisterBroadcast<Message>(OnClientMessageRecieved);
        // ChatInputField.onEndEdit.RemoveListener(delegate { SendMessage(); });
    }

    public void TryFocusInputField() {
        if (IsFocussedOnText()) {
            SendMessage();
        }
        else ChatInputField.Select();
    }
    
    public void SendMessage() {
        if (string.IsNullOrEmpty(ChatInputField.text)) return;
        EventSystem.current.SetSelectedGameObject(null);
        Message msg = new Message() {
            username = "Dummy",
            message = ChatInputField.text
        };
        ChatInputField.text = string.Empty;

        if (InstanceFinder.IsServerStarted) {
            InstanceFinder.ServerManager.Broadcast(msg);
        }
        else if (InstanceFinder.IsClientStarted) {
            InstanceFinder.ClientManager.Broadcast(msg);
        }
    }

    public void SendMessage(Message msg) {
        if (InstanceFinder.IsServerStarted) {
            InstanceFinder.ServerManager.Broadcast(msg);
        }
        else if (InstanceFinder.IsClientStarted) {
            InstanceFinder.ClientManager.Broadcast(msg);
        }
    }

    private void OnMessageRecieved(Message message, Channel channel = Channel.Reliable) {
        var obj = Instantiate(ChatTextObject);
        var rect = obj.transform.GetComponent<RectTransform>();
        rect.SetParent(Content, false);
        rect.SetSiblingIndex(0);
        var textMesh = obj.GetComponent<TextMeshProUGUI>();
        textMesh.text = $"{message.username} : {message.message}";
    }
    
    private void OnClientMessageRecieved(NetworkConnection networkConnection, Message msg, Channel channel = Channel.Reliable)
    {
        InstanceFinder.ServerManager.Broadcast(msg);
    }
}