using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

public class PlayerInitializer : NetworkBehaviour {
    public NetworkObject PlayerPrefab;
    public static bool IsLocalPlayerSpawned;
    private NetworkManager _networkManager;
    private void Awake() {
        InitializeOnce();
    }

    private void Start() {
        if(InstanceFinder.ClientManager.Connection.IsLocalClient) {
            var conn = InstanceFinder.ClientManager.Connection;
            print(PlayerPrefab == null);
            SpawnPlayer(conn);
            IsLocalPlayerSpawned = true;
        }
    }

    private void InitializeOnce()
    {
        _networkManager = InstanceFinder.NetworkManager;
        if (_networkManager == null)
        {
            NetworkManagerExtensions.LogWarning($"PlayerSpawner on {gameObject.name} cannot work as NetworkManager wasn't found on this object or within parent objects.");
            return;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayer(NetworkConnection conn) {
        NetworkObject nob = _networkManager.GetPooledInstantiated(PlayerPrefab, Vector3.zero, Quaternion.identity, true);
        InstanceFinder.NetworkManager.ServerManager.Spawn(nob, conn);
    }
}
