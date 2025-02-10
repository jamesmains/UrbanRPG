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
    private AttributeSkill DebugAttribute;


    private void Awake() {
        OutputText = GetComponent<TextMeshProUGUI>();
        if (DebugAttributeDetails != null) {
            DebugAttribute = new AttributeSkill(DebugAttributeDetails);
            Debug.Log(DebugAttribute.Details == null);
        }
    }

    private void OnEnable() {
        DebugAttribute.Experience.OnValueChanged += SetText;
        DebugAttribute.Level.OnValueChanged += SetText;
    }

    private void OnDisable() {
        DebugAttribute.Experience.OnValueChanged -= SetText;
        DebugAttribute.Level.OnValueChanged -= SetText;
    }

    [Button]
    public void AddExp(int amount) {
        DebugAttribute.AddExperience(amount);
    }

    [Button]
    public void AddLeve() {
        DebugAttribute.AddLevel(1);
    }

    private void SetText<T>(T value) {
        OutputText.text =
            $"Skill: {DebugAttribute.Details.Name}, Level: {DebugAttribute.Level.Value}, Experience: {DebugAttribute.Experience.Value}, Next Level: {DebugAttribute.ExperienceRequired}";
    }
}