
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue Segment", menuName = "Dialogue/Dialogue Segment")]
public class DialogueSegment : SerializedScriptableObject
{
    public bool IsInstantSegment;
    [FoldoutGroup("Details"), HideIf("IsInstantSegment")]public Actor actor;
    [FoldoutGroup("Details"), HideIf("IsInstantSegment",false)][TextArea] public string speech;
    
    [field: SerializeField] private List<Condition> SegmentConditions { get; set; } = new();
    [FoldoutGroup("Events")]public UnityEvent OnStartSegment;
    [FoldoutGroup("Events")]public UnityEvent OnFinishSegment;

    public bool IsConditionMet()
    {
        if (SegmentConditions.Count == 0) return true;
        return SegmentConditions.TrueForAll(c => c.IsConditionMet());
    }
}