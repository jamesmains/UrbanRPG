using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

public class ClientPlayerInitializer : MonoBehaviour {
    [SerializeField] [BoxGroup("Dependencies")]
    private NetworkObject PlayerPrefab;
    private NetworkManager _networkManager;
    public static bool PlayerExists;
    
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
        _networkManager.ClientManager.StartConnection();
        SpawnPlayer(_networkManager.ClientManager.Connection);
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
