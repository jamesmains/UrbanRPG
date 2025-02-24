using System.Collections.Generic;
using Gnomes.Actor.Component;
using Gnomes.Actor.Extension.Interactions;
using Sirenix.OdinInspector;
using UnityEngine;

public enum InteractionDetectionMode {
    _2d,
    _3d
}
public class ActorInteractor : ActorComponent {
    
    [SerializeField,BoxGroup("Settings")]
    private InteractionDetectionMode DetectionMode = InteractionDetectionMode._3d;
    private readonly Dictionary<GameObject, IInteractable> NearbyTriggers = new();

    protected override void OnEnable() {
        base.OnEnable();
        Actor.OnInteract += TryInteract;
    }

    protected override void OnDisable() {
        base.OnDisable();
        Actor.OnInteract -= TryInteract;
    }

    private void TryInteract() {
        print("Trying to interact");
        foreach (var trigger in NearbyTriggers) {
            trigger.Value.Activate();
        }
    }

    private void TryAddInteractable(GameObject targetObj) {
        if (!targetObj.TryGetComponent(out IInteractable pTrigger)) return;
        {
            if (pTrigger.RequiresButtonPressToActivate())
                pTrigger.NotifyEntry();
            else pTrigger.Activate();
            NearbyTriggers.TryAdd(targetObj, pTrigger);
        }
    }

    private void TryRemoveInteractable(GameObject targetObj) {
        if (NearbyTriggers.ContainsKey(targetObj)) {
            if (!NearbyTriggers[targetObj].RequiresButtonPressToDeactivate())
                NearbyTriggers[targetObj].Deactivate();
            NearbyTriggers.Remove(targetObj);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (DetectionMode == InteractionDetectionMode._3d) return;
        TryAddInteractable(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (DetectionMode == InteractionDetectionMode._3d) return;
        TryRemoveInteractable(other.gameObject);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (DetectionMode == InteractionDetectionMode._2d) return;
        TryAddInteractable(other.gameObject);
    }
    
    private void OnTriggerExit(Collider other) {
        if (DetectionMode == InteractionDetectionMode._2d) return;
        TryRemoveInteractable(other.gameObject);
    }
}