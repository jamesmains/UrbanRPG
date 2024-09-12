using System;
using System.Collections;
using System.Linq;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static bool IsLocalPlayerSpawned = false;
    private void Start() {
        if (IsLocalPlayerSpawned) return;
        TrySpawnPlayer();
        InstanceFinder.SceneManager.OnLoadEnd += SetActiveScene;
    }

    private void SetActiveScene(SceneLoadEndEventArgs sceneLoadEndEventArgs) {
        foreach (var scene in sceneLoadEndEventArgs.LoadedScenes) {
            print(scene.name);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
        }
        InstanceFinder.SceneManager.OnLoadEnd -= SetActiveScene;
    }
    private void OnDestroy() {
    }

    private void TrySpawnPlayer() {
        var dummyPlayer = FindObjectsByType<DummyPlayer>(FindObjectsSortMode.None);
        var conn = InstanceFinder.ClientManager.Connection;
        var target = dummyPlayer.FirstOrDefault(o => o.Owner == conn);
        target?.SpawnPlayer(conn);
        IsLocalPlayerSpawned = true;
    }
}
