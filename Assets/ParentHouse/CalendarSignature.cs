using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Calendar Signature", menuName = "Signatures/Calendar Signature")]
    public class CalendarSignature : SerializedScriptableObject {
        public bool Active;
        [FoldoutGroup("Details")] public string DisplayName;
        [FoldoutGroup("Details")] [TextArea] public string DisplayText;
        [FoldoutGroup("Details")] public Sprite DisplayIcon;

        [field: SerializeField] private List<CalendarCondition> Conditions { get; } = new();

        public bool IsConditionMet(int day, int month) {
            return Conditions.TrueForAll(c => c.IsConditionMet(day, month));
        }
    }
}