using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Urban.Urban_Time {
    public class TimeDisplay : MonoBehaviour {
        [SerializeField, FoldoutGroup("Dependencies"), ReadOnly]
        private TextMeshProUGUI DisplayText;

        private void Awake() {
            DisplayText = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable() {
            TimeManager.OnTimeChanged += UpdateTimeDisplay;
        }

        private void OnDisable() {
            TimeManager.OnTimeChanged -= UpdateTimeDisplay;
        }

        private void UpdateTimeDisplay(int min, int hour, int day, int week, int month, int year) {
            DisplayText.text = $"{(Months)month} {TimeManager.AddOrdinal(day)}, {hour.ToString()}:{min.ToString("00")}";
        }
    }
}
