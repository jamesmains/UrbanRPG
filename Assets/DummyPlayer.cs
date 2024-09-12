using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class DummyPlayer : NetworkBehaviour
{
    public GameObject PlayerPrefab;
    public bool IsLocalPlayerSpawned;
    public NetworkObject SelfNob;
    
    public static DummyPlayer Singleton;
    private NetworkManager _networkManager;

    public override void OnStartClient() {
        base.OnStartClient();
        if (Owner == InstanceFinder.ClientManager.Connection)
            Singleton = this;
        InitializeOnce();
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

    [Button]
    [ServerRpc]
    public void SendToScene() {
        SceneLoadData sld = new SceneLoadData("Room 0");
        sld.MovedNetworkObjects = new NetworkObject[] {SelfNob};
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(SelfNob.Owner, sld);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayer(NetworkConnection conn) {
        GameObject nob = Instantiate(PlayerPrefab);
        print(this.transform.parent);
        Spawn(nob, conn);
        Despawn();
    }
}
