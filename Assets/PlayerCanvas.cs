using System;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour {
    public static PlayerCanvas Singleton;
    private void Awake() {
        if (Singleton == null) {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
}
