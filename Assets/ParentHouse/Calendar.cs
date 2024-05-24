using System.Collections.Generic;
using ParentHouse.UI;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    public class Calendar : WindowDisplay {
        [FoldoutGroup("Display")] [SerializeField]
        private TextMeshProUGUI monthNameDisplay;

        [FoldoutGroup("Display")] [SerializeField]
        private Transform calendaryDisplayContainer;

        [FoldoutGroup("Display")] [SerializeField]
        private GameObject calendarDayDisplayObject;

        [FoldoutGroup("Data")] [SerializeField]
        private TimeVariable dayVariable;

        [FoldoutGroup("Data")] [SerializeField]
        private TimeVariable monthVariable;

        public TextMeshProUGUI calendarDayDetailsText;
        public List<CalendarSignature> calendarSignatures = new();

        private readonly List<CalendarDayDisplay> calendarDaySlots = new();

        private void Update() {
            monthNameDisplay.text =
                $"{((Month) monthVariable.Value).ToString()} the {UtilFunctions.AddOrdinal((int) (dayVariable.Value + 1))}";
        }

        protected void OnEnable() {
            GameEvents.OnNewDay.AddListener(UpdateCalendar);
        }

        protected void OnDisable() {
            GameEvents.OnNewDay.AddListener(UpdateCalendar);
        }

        public override void Show() {
            base.Show();
            UpdateCalendar();
        }

        public void UpdateCalendar() {
            for (var day = 0; day < 28; day++) {
                if (calendarDaySlots.Count - 1 < day || calendarDaySlots[day] == null) {
                    var obj = Instantiate(calendarDayDisplayObject, calendaryDisplayContainer)
                        .GetComponent<CalendarDayDisplay>();
                    calendarDaySlots.Add(obj);
                }

                var isHighlighted = day == (int) dayVariable.Value;
                var signaturesPerDayCount = 0;
                var detailString = "";

                detailString +=
                    $"<b><u>{((Month) monthVariable.Value).ToString()} the {UtilFunctions.AddOrdinal(day + 1)}</b></u><br><br>";
                foreach (var signature in calendarSignatures) {
                    if (signature.Active == false) continue;
                    if (signature.IsConditionMet(day, -1)) {
                        detailString += $"<b>{signature.DisplayName}</b>";
                        detailString += $"{signature.DisplayText}<br>";
                        signaturesPerDayCount++;
                    }
                }

                var display = calendarDaySlots[day];

                calendarDayDetailsText.text = "";
                display.Setup(isHighlighted, day, signaturesPerDayCount > 0, this);
                display.AssignInteract(delegate {
                    calendarDayDetailsText.text = detailString;
                    display.ToggleTemporaryHighlight(true);
                });
                if (isHighlighted) {
                    display.ToggleTemporaryHighlight(true);
                    display.Interact();
                }
            }
        }

#if UNITY_EDITOR
        [Button]
        public void FindAssetsByType() {
            calendarSignatures.Clear();
            var assets = AssetDatabase.FindAssets("t:CalendarSignature");

            foreach (var t in assets) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<CalendarSignature>(assetPath);
                calendarSignatures.Add(asset);
            }
        }

        [Button]
        private void ClearDisplays() {
            calendaryDisplayContainer.DestroyChildrenInEditor();
            calendarDaySlots.Clear();
        }
#endif
    }
}