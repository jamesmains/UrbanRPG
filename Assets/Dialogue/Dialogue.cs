using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Unsorted/Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueSegment[] dialogueSegments;
}

[Serializable]
public class DialogueSegment
{
    [FoldoutGroup("Details")]public Actor actor;
    [FoldoutGroup("Details")][TextArea] public string speech;
    [FoldoutGroup("Events")]public UnityEvent OnStartSegment;
    [FoldoutGroup("Events")]public UnityEvent OnFinishSegment;
}
