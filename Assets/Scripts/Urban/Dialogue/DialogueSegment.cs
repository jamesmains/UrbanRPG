using System;
using System.Collections.Generic;
using System.Linq;
using Gnomes.Actor;
using Parent_House_Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Urban.Dialogue {
    public class DialogueSegment {
        public enum SegmentType {
            Standard,
            RequiresOptions,
            EndConversation
        }

        [SerializeField]
        public string Name;

        [SerializeField] [ValueDropdown(nameof(GetSegmentGuids))]
        private Guid NextSegment;

        [SerializeField] [ValueDropdown(nameof(GetSegmentGuids))]
        private Guid FallbackSegment = Guid.Empty;

        [SerializeField, TextArea]
        private string Content;

        [SerializeField, FoldoutGroup("Settings")]
        private ActorDetails Speaker;
    
        [SerializeField, FoldoutGroup("Settings")]
        public Condition Conditions;

        [SerializeField, FoldoutGroup("Settings")]
        public SegmentType Type;

        [SerializeField, FoldoutGroup("Settings")]
        private DialogueBoxSettings DialogueBoxSettings;

        [SerializeField, FoldoutGroup("Settings"), ShowIf("Type",SegmentType.RequiresOptions)]
        public List<DialogueOption> Options = new();

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        public Guid Id;

        [SerializeField, FoldoutGroup("Status"), ReadOnly]
        private Dialogue Owner;

        public bool EndsConversation() => Type == SegmentType.EndConversation;
        public string GetContent() => Content;
        public string GetSpeakerName() => Speaker.ActorName;
        public DialogueBoxSettings GetDialogueBoxSettings() => DialogueBoxSettings;

        // Filter out options until something useful is returned.
        public List<DialogueOption> GetOptions() {
            if (Type != SegmentType.RequiresOptions || Options == null || Options.Count <= 0) return null;
            var filteredList = Options.Where(o => o.Conditions == null || o.Conditions.IsConditionMet()).ToList();
            return filteredList.Count <= 0 ? null : filteredList;
        }

        public DialogueSegment GetNextSegment() {
            DialogueSegment nextSegment = Owner.GetSegmentById(NextSegment);
            if (nextSegment.Conditions != null)
                nextSegment = nextSegment.Conditions.IsConditionMet() ? nextSegment : Owner.GetSegmentById(FallbackSegment);
            return nextSegment;
        }

        public void SetOwner(Dialogue owner) {
            Owner = owner;
            if (Id == Guid.Empty) {
                Id = Guid.NewGuid();
            }
        }

        private IEnumerable<ValueDropdownItem> GetSegmentGuids() {
            return Owner.Segments
                .Select(segment => new ValueDropdownItem($"{segment.Name} ({segment.Id})", segment.Id));
        }
    }
}