using System;
using System.Collections.Generic;
using System.Linq;
using Parent_House_Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Urban.Dialogue {
    public class DialogueOption {
        // Dialogue Option
// Parameters
        [SerializeField]
        public string OptionName;

        [SerializeField]
        private Dialogue TargetDialogue;
    
        [SerializeField,ValueDropdown(nameof(GetTargetDialogueSegmentGuids))]
        private Guid TargetSegmentGuid;
    
        [SerializeField]
        private Dialogue FallbackDialogue;
    
        [SerializeField,ValueDropdown(nameof(GetFallbackDialogueSegmentGuids))]
        private Guid FallbackSegmentGuid;
    
        [SerializeField, FoldoutGroup("Settings")]
        public Condition Conditions;
    
        public Tuple<Dialogue,DialogueSegment> GetTargetDialogueAndSegment() {
        
            Dialogue nextDialogue = TargetDialogue;
            DialogueSegment nextSegment = TargetDialogue.GetSegmentById(TargetSegmentGuid);
            if (nextSegment.Conditions != null && nextSegment.Conditions.IsConditionMet()) {
                nextDialogue = FallbackDialogue;
                nextSegment = FallbackDialogue.GetSegmentById(FallbackSegmentGuid);
            }
            return new(nextDialogue,nextSegment);
        }
    
        private IEnumerable<ValueDropdownItem> GetTargetDialogueSegmentGuids() {
            if(TargetDialogue == null) return Enumerable.Empty<ValueDropdownItem>();
            return TargetDialogue.Segments
                .Select(segment => new ValueDropdownItem($"{segment.Name} ({segment.Id})", segment.Id));
        }

        private IEnumerable<ValueDropdownItem> GetFallbackDialogueSegmentGuids() {
            if(FallbackDialogue == null) return Enumerable.Empty<ValueDropdownItem>();
            return FallbackDialogue.Segments
                .Select(segment => new ValueDropdownItem($"{segment.Name} ({segment.Id})", segment.Id));
        }
    }
}