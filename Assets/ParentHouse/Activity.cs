using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Activity", menuName = "Signatures/Activity")]
    public class Activity : SerializedScriptableObject {
        // [FoldoutGroup("Activity Details"),PropertyOrder(70)] public ActionType actionType;
        [Title("Activity Settings")] [PropertyOrder(70)]
        public string ActivityName;

        [PropertyOrder(70)] [PreviewField] public Sprite ActivityIcon;
        [PropertyOrder(70)] public ActivityType ActivityType;
        [PropertyOrder(70)] public float actionTime;

        [field: SerializeField]
        [field: PropertyOrder(90)]
        [field: Space(10)]
        public UnityEvent ActivityActions { get; private set; } = new();

        [PropertyOrder(70)]
        public float ActionTime => actionTime - 0; // TODO: Replace '- 0' with some kind of stat or status modifier

        [field: SerializeField]
        [field: PropertyOrder(80)]
        [field: Space(10)]
        private List<Condition> Conditions { get; } = new();

        public bool IsConditionMet() {
            if (Conditions.Count == 0) return true;
            return Conditions.TrueForAll(c => c.IsConditionMet());
        }

        public void InvokeActivity() {
            ActivityActions.Invoke();
            Conditions.ForEach(c => c.Use());
        }
    }
}