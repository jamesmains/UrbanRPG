using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableResourceNode : NetworkBehaviour, Interactable {
    [SerializeField] [FoldoutGroup("Settings")]
    private int InteractionPriority = 10;

    public int Priority => InteractionPriority;

    [SerializeField] [FoldoutGroup("Settings")]
    public Item NodeItem;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    public SpriteRenderer Renderer;

    private readonly SyncVar<bool> IsActive = new();

    private void SetState(bool state) => IsActive.Value = state;

    private void OnChangeState(bool prev, bool next, bool asServer) {
        Renderer.color = !next ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
    }

    private void Awake() {
        Renderer = GetComponent<SpriteRenderer>();
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
        Inventory.AddItem(NodeItem);
    }

    public void ExitInteraction() {
        // throw new System.NotImplementedException();
    }
}