using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using urban.Job;

// In the moment debugger
public class ItmDebugger : MonoBehaviour {
    [SerializeField, BoxGroup("Settings")]
    private JobDetails DebugJobOneDetails;
    
    [SerializeField, BoxGroup("Settings")]
    private TextMeshProUGUI JobTitleOneText;
    
    [SerializeField, BoxGroup("Settings")]
    private TextMeshProUGUI DaysWorkedOneText;

    [SerializeField, BoxGroup("Dependencies"), ReadOnly]
    private TextMeshProUGUI OutputText;
    
    private void Awake() {
        OutputText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() {
        // TimeManager.OnHourChanged += SetText;
        JobTitleOneText.text = DebugJobOneDetails.Title;
        DaysWorkedOneText.text = $"{DebugJobOneDetails.GetDaysWorkedString()}\n{DebugJobOneDetails.GetHolidaysString()}";
    }

    private void OnDisable() {
    }

    private void SetText() {
    }
}