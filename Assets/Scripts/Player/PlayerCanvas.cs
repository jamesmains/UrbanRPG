using System;
using FishNet;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCanvas : MonoBehaviour {
    [FormerlySerializedAs("Group")] [SerializeField] [BoxGroup("Dependencies")]
    private CanvasGroup CanvasGroup;
    public static PlayerCanvas Singleton;

    private void Awake() {
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
        if(Singleton != null){
            Destroy(this.gameObject);
        }
        else {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void HookToPlayer() {
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }
}
