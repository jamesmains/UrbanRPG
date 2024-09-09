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
    [SerializeField] [BoxGroup("Dependencies")]
    private NetworkObject PlayerPrefab;

    private NetworkManager _networkManager;


    private void Start() {
        InitializeOnce();
        // SpawnThis();
    }

    private void Update() {
        print(InstanceFinder.NetworkManager.ClientManager.Connection);
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

    public void EnterWorld() {
        SpawnPlayer(InstanceFinder.NetworkManager.ClientManager.Connection);
        print($"Try enter world. Server: {InstanceFinder.ServerManager.AnyServerStarted()}");
        if (CurrentPlayerInfo.Data == null) {
            Debug.LogError("Client Handler: Player Data is null");
            return;
        }
        
        var targetScene = CurrentPlayerInfo.Data.Room;
        SceneLoadData sld = new SceneLoadData(targetScene);
        // sld.MovedNetworkObjects = new NetworkObject[] {nob};
        sld.ReplaceScenes = ReplaceOption.All;
        var conn = InstanceFinder.ClientManager.Connection;
        InstanceFinder.SceneManager.LoadConnectionScenes(sld);
    }
    private void SpawnPlayer(NetworkConnection conn) {
        if (PlayerPrefab == null)
        {
            NetworkManagerExtensions.LogWarning($"Player prefab is empty and cannot be spawned for connection {conn.ClientId}.");
            return;
        }

        Vector3 position = CurrentPlayerInfo.Data.Position;

        NetworkObject nob = _networkManager.GetPooledInstantiated(PlayerPrefab, position, Quaternion.identity, true);
        _networkManager.ServerManager.Spawn(nob, conn);
        _networkManager.SceneManager.AddOwnerToDefaultScene(nob);
            
    }
}