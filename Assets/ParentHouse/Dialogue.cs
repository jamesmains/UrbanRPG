using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Signatures/Dialogue")]
    public class Dialogue : SerializedScriptableObject {
        [FoldoutGroup("Events")] public UnityEvent OnStartDialogue;
        [FoldoutGroup("Events")] public UnityEvent OnFinishDialogue;
        [OdinSerialize] public List<DialogueSegment> DialogueSegments = new();
        [field: SerializeField] private List<Condition> DialogueConditions { get; } = new();

        public bool IsConditionMet() {
            if (DialogueConditions.Count == 0) return true;
            return DialogueConditions.TrueForAll(c => c.IsConditionMet());
        }

        public void StartDialogue() {
            OnStartDialogue.Invoke();
            GameEvents.StartDialogueEvent.Invoke(this);
        }

        public void EndDialogue() {
            OnFinishDialogue.Invoke();
        }
    }


    public class DialogueSegment {
        [Space(10)] [HideIf("IsInstantSegment")]
        public Actor actor;

        [Space(10)] public bool IsInstantSegment;

        public UnityEvent OnFinishSegment;

        [Space(10)] public UnityEvent OnStartSegment;

        [HideIf("IsInstantSegment", false)] [TextArea]
        public string speech;

        [ShowInInspector] [OdinSerialize] public List<Condition> SegmentConditions { get; set; } = new();

        public bool IsConditionMet() {
            if (SegmentConditions.Count == 0) return true;
            return SegmentConditions.TrueForAll(c => c.IsConditionMet());
        }
    }
}