using System;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using TMPro;
using UnityEngine;

public class ServerInitializer : NetworkBehaviour {
    public TextMeshProUGUI DebugText;
    
    void Update()
    {
        DebugText.text = "";
        DebugText.text += $"Any Server Started. {InstanceFinder.ServerManager.AnyServerStarted()}\n"; 
        DebugText.text += $"Server Started. {InstanceFinder.ServerManager.Started}\n"; 
        DebugText.text += $"Network Manager Initialized. {InstanceFinder.NetworkManager.Initialized}\n"; 
        // if (InstanceFinder.ServerManager.AnyServerStarted() == false) {
        //     InstanceFinder.ServerManager.StartConnection();
        //     print("Started connection");
        // }
        // else Destroy(this);
    }
}
