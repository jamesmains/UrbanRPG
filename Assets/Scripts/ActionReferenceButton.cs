using System;
using System.Collections.Generic;
using gnomes;
using parent_house_framework.Conditions;
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

    private void TryInvokeButton(InputAction.CallbackContext callbackContext, InputAction inputAction) {
        if (inputAction.name != TargetActionReference.action.name ||
            (ActionCondition != null && !ActionCondition.IsConditionMet())) {
            return;
        }
        OnInvokeAction?.Invoke();
    }
}