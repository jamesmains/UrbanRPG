using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Unsorted/Dialogue")]
public class Dialogue : SerializedScriptableObject
{
    public DialogueSegment[] dialogueSegments;
    [field: SerializeField] private List<Condition> DialogueConditions { get; set; } = new();
    public bool IsConditionMet()
    {
        if (DialogueConditions.Count == 0) return true;
        return DialogueConditions.TrueForAll(c => c.IsConditionMet());
    }
}

[Serializable]
public class DialogueSegment
{
    public bool IsInstantSegment;
    [FoldoutGroup("Details"), HideIf("IsInstantSegment")]public Actor actor;
    [FoldoutGroup("Details"), HideIf("IsInstantSegment",false)][TextArea] public string speech;
    [ShowInInspector]
    [field: SerializeField] private List<Condition> SegmentConditions { get; set; } = new();
    [FoldoutGroup("Events")]public UnityEvent OnStartSegment;
    [FoldoutGroup("Events")]public UnityEvent OnFinishSegment;

    public bool IsConditionMet()
    {
        if (SegmentConditions.Count == 0) return true;
        return SegmentConditions.TrueForAll(c => c.IsConditionMet());
    }
}
