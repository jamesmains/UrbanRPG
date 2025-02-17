using System;
using System.Collections.Generic;
using Gnomes;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionReferenceButton : SerializedMonoBehaviour {
    [SerializeField, FoldoutGroup("Settings")]
    private Condition ActionCondition;
    [SerializeField, FoldoutGroup("Settings")]
    public InputActionReference TargetActionReference;

    [SerializeField, FoldoutGroup("Events")]
    private UnityEvent OnInvokeAction = new();

    private void OnEnable() {
        Player.OnButtonPressed += TryInvokeButton;
    }

    private void OnDisable() {
        Player.OnButtonPressed -= TryInvokeButton;
    }

    private void TryInvokeButton(InputAction inputAction) {
        if (inputAction.name != TargetActionReference.action.name || (ActionCondition != null && !ActionCondition.IsConditionMet())) {
            Debug.Log($"Action {inputAction.name} does not equal {TargetActionReference.action.name}");
            return;
        }
        Debug.Log($"DoThing for {TargetActionReference.name}");
        OnInvokeAction?.Invoke();
    }
}
