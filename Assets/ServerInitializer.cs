using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using TMPro;
using UnityEngine;

public class ServerInitializer : MonoBehaviour {
    public NetworkObject ClientLoginHandler;
    public TextMeshProUGUI DebugText;

    private NetworkManager _networkManager;
    
    private void Start() {
        InitializeOnce();
    }

    private void InitializeOnce() {
        _networkManager = InstanceFinder.NetworkManager;
        if (_networkManager == null) {
            NetworkManagerExtensions.LogWarning(
                $"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
            return;
        }
        _networkManager.ServerManager.StartConnection();
        InstanceFinder.ServerManager.OnServerConnectionState += SpawnClientLoginHandler;
    }
    
    private void OnDestroy() {
        InstanceFinder.ServerManager.OnServerConnectionState -= SpawnClientLoginHandler;
    }
    
    private void SpawnClientLoginHandler(ServerConnectionStateArgs serverConnectionStateArgs) {

        if (serverConnectionStateArgs.ConnectionState != LocalConnectionState.Started) return;
        Vector3 position = CurrentPlayerInfo.Data.Position;

        NetworkObject nob = _networkManager.GetPooledInstantiated(ClientLoginHandler, position, Quaternion.identity, true);
        _networkManager.ServerManager.Spawn(nob, _networkManager.ClientManager.Connection);
        _networkManager.SceneManager.AddOwnerToDefaultScene(nob);
            
    }

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
