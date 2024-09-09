using System;
using System.Collections;
using FishNet;
using FishNet.Managing.Scened;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour {
#if UNITY_EDITOR
    [SerializeField] [FoldoutGroup("Dependencies")]
    private SceneAsset TargetScene;

    private void OnValidate() {
        if (TargetScene != null)
            TargetSceneName = TargetScene.name;
    }
#endif

    [SerializeField] [FoldoutGroup("Status")]
    private string TargetSceneName;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private bool IsActive;

    private void Awake() {
        IsActive = false;
        StartCoroutine(DelayActivate());

        IEnumerator DelayActivate() {
            yield return new WaitForSeconds(1f);
            IsActive = true;
        }
    }

    private void LoadScene(NetworkObject nob) {
        if (!nob.Owner.IsActive)
            return;

        SceneLoadData sld = new SceneLoadData(TargetSceneName);
        sld.MovedNetworkObjects = new NetworkObject[] {nob};
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadConnectionScenes(nob.Owner, sld);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!IsActive) return;
        if (other.TryGetComponent(out NetworkObject nob)) {
            LoadScene(nob);
        }
    }
}