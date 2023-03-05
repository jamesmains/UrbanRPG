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
    [SerializeField] private DialogueGameEvent StartDialogueEvent;
    [OdinSerialize] public List<DialogueSegment> DialogueSegments = new();
    [field: SerializeField] private List<Condition> DialogueConditions { get; set; } = new();
    
    public bool IsConditionMet()
    {
        if (DialogueConditions.Count == 0) return true;
        return DialogueConditions.TrueForAll(c => c.IsConditionMet());
    }

    public void StartDialogue()
    {
        StartDialogueEvent.Raise(this);
    }
    
#if UNITY_EDITOR
    [Button]
    public void SetDialogueStartVariable()
    {
        var quests = AssetDatabase.FindAssets("t:DialogueGameEvent");
        
        for (int i = 0; i < quests.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( quests[i] );
            var asset = AssetDatabase.LoadAssetAtPath<DialogueGameEvent>( assetPath );
            StartDialogueEvent = asset;
        }
    }    
#endif

}


public class DialogueSegment
{
    public bool IsInstantSegment;
    [FoldoutGroup("Details"), HideIf("IsInstantSegment")]public Actor actor;
    [FoldoutGroup("Details"), HideIf("IsInstantSegment",false)][TextArea] public string speech;
    
    [ShowInInspector,OdinSerialize] public List<Condition> SegmentConditions { get; set; } = new();
    [FoldoutGroup("Events")]public UnityEvent OnStartSegment;
    [FoldoutGroup("Events")]public UnityEvent OnFinishSegment;

    public bool IsConditionMet()
    {
        if (SegmentConditions.Count == 0) return true;
        return SegmentConditions.TrueForAll(c => c.IsConditionMet());
    }
}