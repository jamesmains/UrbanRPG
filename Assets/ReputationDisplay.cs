using System;
using gnomes.Actor;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ReputationDisplay : MonoBehaviour {
    [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
    private TextMeshProUGUI DisplayText;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private ActorDetails AssociatedActor;
    
    private void Awake() {
        DisplayText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable() {
        ReputationManager.OnReputationChanged += HandleReputationChanged;
    }

    private void OnDisable() {
        ReputationManager.OnReputationChanged -= HandleReputationChanged;
    }

    public ReputationDisplay Build(ActorDetails actor) {
        AssociatedActor = actor;
        return this;
    }
    
    // Currently this only is called when the tier is changed
    private void HandleReputationChanged(ActorDetails details, int value) {
        if (details != AssociatedActor) return;
        DisplayText.text = $"{details.ActorName}, {ReputationTiers.GetTierName(value)}";
    }
}
