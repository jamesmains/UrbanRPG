using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] [FoldoutGroup("Settings")]
    protected List<HoverableBehaviourUi> UiHoverBehaviours = new();

    private void Awake() {
        foreach (var behaviour in UiHoverBehaviours) {
            behaviour.OnHoverExit();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        foreach (var behaviour in UiHoverBehaviours) {
            behaviour.OnHoverEnter();
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        foreach (var behaviour in UiHoverBehaviours) {
            behaviour.OnHoverExit();
        }
    }
}

[Serializable]
public abstract class HoverableBehaviourUi {
    public abstract void OnHoverEnter();
    public abstract void OnHoverExit();
}

/// <summary>
/// TargetObject: 
/// </summary>
public class ToggleObjectOnHover : HoverableBehaviourUi {
    [SerializeField] [BoxGroup("Dependencies")] 
    private GameObject TargetObject;
    public override void OnHoverEnter() {
        TargetObject.SetActive(true);
    }

    public override void OnHoverExit() {
        TargetObject.SetActive(false);
    }
}

public class ShowTextOnHover : HoverableBehaviourUi {
    [SerializeField] [BoxGroup("Dependencies")]
    private TextMeshProUGUI Text;
    [SerializeField] [BoxGroup("Dependencies")]
    public string TextToDisplay;
    public override void OnHoverEnter() {
        Text.text = TextToDisplay;
    }

    public override void OnHoverExit() {
        Text.text = "";
    }
}