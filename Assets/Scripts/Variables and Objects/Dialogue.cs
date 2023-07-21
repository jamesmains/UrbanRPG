using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class Dialogue : SerializedScriptableObject
{
    [OdinSerialize] public List<DialogueSegment> DialogueSegments = new();
    [FoldoutGroup("Events")]public UnityEvent OnStartDialogue;
    [FoldoutGroup("Events")]public UnityEvent OnFinishDialogue;
    [field: SerializeField] private List<Condition> DialogueConditions { get; set; } = new();
    
    public bool IsConditionMet()
    {
        if (DialogueConditions.Count == 0) return true;
        return DialogueConditions.TrueForAll(c => c.IsConditionMet());
    }

    public void StartDialogue()
    {
        OnStartDialogue.Invoke();
        GameEvents.StartDialogueEvent.Raise(this);
    }

    public void EndDialogue()
    {
        OnFinishDialogue.Invoke();
    }

}


public class DialogueSegment
{
    [Space(10)]
    public bool IsInstantSegment;
    
    [Space(10)]
    [HideIf("IsInstantSegment")]public Actor actor;
    [HideIf("IsInstantSegment",false)][TextArea] public string speech;
    [ShowInInspector,OdinSerialize] public List<Condition> SegmentConditions { get; set; } = new();
    
    [Space(10)]
    public UnityEvent OnStartSegment;
    public UnityEvent OnFinishSegment;

    public bool IsConditionMet()
    {
        if (SegmentConditions.Count == 0) return true;
        return SegmentConditions.TrueForAll(c => c.IsConditionMet());
    }
}