using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

// In the moment debugger
public class ItmDebugger : MonoBehaviour {
    [SerializeReference, BoxGroup("Settings")]
    private AttributeDetails DebugAttributeDetails;

    [SerializeReference, BoxGroup("Dependencies"), ReadOnly]
    private TextMeshProUGUI OutputText;

    [SerializeReference, BoxGroup("Status"), ReadOnly]
    private AttributeNeed DebugAttribute;


    private void Awake() {
        OutputText = GetComponent<TextMeshProUGUI>();
        if (DebugAttributeDetails != null) {
            DebugAttribute = new AttributeNeed(DebugAttributeDetails);
        }
    }

    private void OnEnable() {
        DebugAttribute.NeedValue.OnValueChanged += SetText;
        DebugAttribute.OnReachedFull += delegate{Debug.Log("FULL");};
        DebugAttribute.OnReachedGood += delegate{Debug.Log("GOOD");};
        DebugAttribute.OnReachedLow += delegate{Debug.Log("LOW");};
        DebugAttribute.OnReachedCritical += delegate{Debug.Log("CRITICAL");};
    }

    private void OnDisable() {
        DebugAttribute.NeedValue.OnValueChanged -= SetText;
    }

    private void Update() {
        DebugAttribute.Decay();
    }

    private void SetText<T>(T value) {
        OutputText.text =
            $"Need Name: {DebugAttributeDetails.Name}, Value: {DebugAttribute.NeedValue.Value}, State: {DebugAttribute.CurrentState}";
    }

    [Button]
    private void SetNeedValueTo(float value) {
        DebugAttribute.NeedValue.Value = value;
    }
}