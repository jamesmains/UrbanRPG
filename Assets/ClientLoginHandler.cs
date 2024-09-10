using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ClientLoginHandler : MonoBehaviour {

    private NetworkManager _networkManager;


    private void Start() {
        InitializeOnce();
    }

    private void Update() {
        print(_networkManager.ServerManager.Started);
        print(_networkManager.ServerManager.Clients.Count);
    }

    private void InitializeOnce() {
        _networkManager = InstanceFinder.NetworkManager;
        if (_networkManager == null) {
            NetworkManagerExtensions.LogWarning(
                $"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
            return;
        }
        _networkManager.ClientManager.StartConnection();
    }

    // [ServerRpc(RequireOwnership = false)]
    public void EnterWorld() {
        print($"Try enter world. Server: {InstanceFinder.ServerManager.AnyServerStarted()}");
        if (CurrentPlayerInfo.Data == null) {
            Debug.LogError("Client Handler: Player Data is null");
            return;
        }
        
        // var targetScene = CurrentPlayerInfo.Data.Room;
        // SceneLoadData sld = new SceneLoadData("Room 0");
        // sld.ReplaceScenes = ReplaceOption.All;
        // var conn = InstanceFinder.ClientManager.Connection;
        // InstanceFinder.SceneManager.LoadConnectionScenes(conn,sld);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}