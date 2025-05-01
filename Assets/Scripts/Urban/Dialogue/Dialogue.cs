using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Urban.Dialogue {
    /// <summary>
    /// Todo: Need to fix function to allow options to work
    /// </summary>
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
    public class Dialogue : SerializedScriptableObject {
        [SerializeField, FoldoutGroup("Settings")]
        [ListDrawerSettings(ListElementLabelName = "Name")]
        public List<DialogueSegment> Segments = new();

        [SerializeField, FoldoutGroup("Status"),ReadOnly]
        private DialogueSegment CurrentSegment;
#if UNITY_EDITOR
        private void OnValidate() {
            foreach (var segment in Segments) {
                segment.SetOwner(this);
            }
        }
#endif
        public DialogueSegment GetFirstSegment() {
            CurrentSegment = Segments[0];
            return Segments[0];
        }

        public Tuple<DialogueSegment,List<DialogueOption>> GetNextSegment() {
            if (CurrentSegment.EndsConversation()) return null;
            CurrentSegment = CurrentSegment.GetNextSegment();
            Tuple<DialogueSegment, List<DialogueOption>> segmentInfo = new(CurrentSegment, CurrentSegment.GetOptions());
            return segmentInfo;
        }

        public DialogueSegment GetSegmentById(Guid id) {
            return Segments.Find(segment => segment.Id == id);
        }
    }
}