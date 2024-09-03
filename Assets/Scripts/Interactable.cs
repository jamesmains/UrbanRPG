using System;
using System.Collections;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using Sirenix.OdinInspector;
using UnityEngine;

public class Interactable : NetworkBehaviour {
    public Item DebugItem;
    
    private readonly SyncVar<bool> IsActive = new();
    public SpriteRenderer Renderer;

    private void SetState(bool state) => IsActive.Value = state;

    private void OnChangeState(bool prev, bool next, bool asServer) {
        Renderer.color = !next ? new Color(1, 1, 1, 0.5f) : new Color(1,1,1,1);
    }

    private void Awake() {
        IsActive.OnChange += OnChangeState;
    }

    public override void OnStartServer() {
        base.OnStartServer();
        IsActive.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryUse() {
        if (!IsActive.Value) return;
        SetState(false);
        StartCoroutine(RespawnObject());
    }
    
    IEnumerator RespawnObject() {
        print("Interacting");
        yield return new WaitForSeconds(1);
        print("Finished interaction");
        SetState(true);
    }
    
    public void Interact() {
        TryUse();
        Inventory.AddItem(DebugItem);
    }
}
