using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

public class ServerInitializer : NetworkBehaviour {
    [SerializeField] [BoxGroup("Dependencies")]
    private GameObject GlobalDataManager;
    public override void OnStartNetwork() {
        base.OnStartNetwork();
        if (!IsServer) return;
        
        SpawnServerDataManager();
    }

    private void SpawnServerDataManager() {
        GameObject nob = Instantiate(GlobalDataManager);
        Spawn(nob, null);
        Despawn();
    }
}
