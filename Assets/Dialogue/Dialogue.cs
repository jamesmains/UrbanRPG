using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueSegment[] dialogueSegments;
}

[Serializable]
public class DialogueSegment
{
    public Actor actor;
    [TextArea] public string speech;
}
