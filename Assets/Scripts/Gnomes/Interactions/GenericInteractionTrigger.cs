using Gnomes.Actor.Extension.Interactions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteractionTrigger : MonoBehaviour, IInteractable {
    [SerializeField, FoldoutGroup("Settings")]
    private InteractionSettings Settings;
    
    [SerializeField] [FoldoutGroup("Status"),ReadOnly]
    public bool Activated;
    
    [SerializeField] [FoldoutGroup("Events")]
    private UnityEvent OnActivate;

    [SerializeField] [FoldoutGroup("Events")]
    private UnityEvent OnDeactivate;

    private void OnEnable() {
        if (Settings.ActiveOnEnable)
            Activate();
        else Deactivate();
    }
    
    public bool RequiresButtonPressToActivate() {
        return Settings.RequireKeyToActivate;
    }
    
    public bool RequiresButtonPressToDeactivate() {
        return Settings.RequireKeyToDeactivate;
    }

    public void NotifyEntry() {
        Debug.Log("GenericInteractionTrigger.NotifyEntry");
    }

    public void NotifyExit() {
        Debug.Log("GenericInteractionTrigger.NotifyExit");
    }

    public void Activate() {
        if (Settings.ActivationSetting == ActivationType.Once && Activated) return;
        if (Settings.ActivationSetting == ActivationType.Toggle && Activated) Deactivate();
        else {
            Debug.Log("GenericInteractionTrigger.Activate");
            OnActivate.Invoke();
            Activated = true;
        }
    }

    public void Deactivate() {
        if (Settings.DeactivationSetting == ActivationType.Once && !Activated) return;
        if (Settings.DeactivationSetting == ActivationType.Toggle && !Activated) Activate();
        else {
            Debug.Log("GenericInteractionTrigger.Deactivate");
            OnDeactivate.Invoke();
            Activated = false;
        }
    }
}