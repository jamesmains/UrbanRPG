using System;
using System.Collections;
using System.Collections.Generic;
using Gnomes.Actor;
using Parent_House_Framework.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public enum DialogueState {
    ReadyForNext,
    WaitingForType,
    Typing,
    WaitingForReady,
    Idle
}

public class DialogueController : MonoBehaviour {
    // Current issues:
    /// <summary>
    /// 1.) Very clunky way to type out text
    /// 2.) Doesn't handle any logic in terms of actionable options for the player
    /// </summary>
    [SerializeField, FoldoutGroup("Debug")]
    public List<DialogueSequence> TestDialogues = new();

    // Todo: grab on awake?
    [SerializeField, FoldoutGroup("Dependencies")]
    private Menu DialogueMenu;

    [SerializeField, FoldoutGroup("Dependencies")]
    private TextMeshProUGUI SequenceOwnerNameText;

    [SerializeField, FoldoutGroup("Dependencies")]
    private TextMeshProUGUI SequenceContentText;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private DialogueState CurrentState;

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private DialogueSequence CurrentSequence;

    private Queue<DialogueSequence> DialogueQueue = new();

    [Button]
    public void StartDialogue() {
        DialogueMenu.Open();
        DialogueQueue = new Queue<DialogueSequence>(TestDialogues);
        StartCoroutine(EnumerateSequence());
    }

    [Button]
    public void PingNextDialogue() {
        CurrentState = DialogueState.ReadyForNext;
    }

    public IEnumerator EnumerateSequence() {
        CurrentState = DialogueState.ReadyForNext;
        while (DialogueQueue.Count > 0) {
            CheckState();
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator StringPrintToText(TextMeshProUGUI text, string textToPrint) {
        int textLength = textToPrint.Length;
        for (int i = 0; i <= textLength; i++) {
            text.text = textToPrint.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForEndOfFrame();
    }

    private void CheckState() {
        if (CurrentState == DialogueState.ReadyForNext) {
            StartNext();
        }
        else if (CurrentState == DialogueState.WaitingForType) {
            ProcessCurrent();
        }
        else if (CurrentState == DialogueState.Typing) {
            // Accept input to kick to next sequence
        }
        else {
        }
    }

    private void StartNext() {
        CurrentSequence = DialogueQueue.Dequeue();
        CurrentState = DialogueState.WaitingForType;
        SequenceOwnerNameText.text = "";
        SequenceContentText.text = "";
    }

    private void ProcessCurrent() {
        StartCoroutine(StringPrintToText(SequenceOwnerNameText, CurrentSequence.OwnerActor.ActorName));
        StartCoroutine(StringPrintToText(SequenceContentText, CurrentSequence.SequenceContent));
        CurrentState = DialogueState.Typing;
    }

    public void EndDialogue() {
        DialogueMenu.Close();
    }
}

[Serializable]
public class DialogueSequence {
    public ActorDetails OwnerActor;

    public string SequenceContent;
    // How do actions? Def need conditionals
}