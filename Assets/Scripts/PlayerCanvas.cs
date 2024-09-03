using System;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour {
    public static PlayerCanvas Singleton;

    public void HookToPlayer() {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
}
