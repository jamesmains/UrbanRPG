using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

public class DummyPlayer : NetworkBehaviour
{
    public NetworkObject nob;
    public static DummyPlayer Singleton;
    public override void OnStartClient() {
        base.OnStartClient();
        if (Owner == InstanceFinder.ClientManager.Connection)
            Singleton = this;
    }

    [Button]
    [ServerRpc]
    public void SendToScene() {
        SceneLoadData sld = new SceneLoadData("Room 0");
        sld.MovedNetworkObjects = new NetworkObject[] {nob};
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
    }
}
