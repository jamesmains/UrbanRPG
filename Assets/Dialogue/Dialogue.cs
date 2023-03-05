using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
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