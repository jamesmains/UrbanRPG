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
    private AttributeStat DebugAttribute;


    private void Awake() {
        OutputText = GetComponent<TextMeshProUGUI>();
        if (DebugAttributeDetails != null) {
            DebugAttribute = new AttributeStat(DebugAttributeDetails);
        }
    }

    private void OnEnable() {
        DebugAttribute.StatValue.OnValueChanged += SetText;
    }

    private void OnDisable() {
        DebugAttribute.StatValue.OnValueChanged -= SetText;
    }

    private void SetText<T>(T value) {
        OutputText.text =
            $"Need Name: {DebugAttributeDetails.Name}, Value: {DebugAttribute.StatValue.Value}";
    }

    [Button]
    private void SetValueTo(int value) {
        DebugAttribute.StatValue.Value = value;
    }

    [Button]
    private void AddToValue() {
        DebugAttribute.StatValue.Value++;
    }

    [Button]
    private void RemoveFromValue() {
        DebugAttribute.StatValue.Value--;
    }
}