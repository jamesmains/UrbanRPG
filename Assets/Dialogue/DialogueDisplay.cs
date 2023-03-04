using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    [FoldoutGroup("Data")][SerializeField] private KeyCode interactionKey; // todo replace with scriptable objects for key rebinding
    [FoldoutGroup("Data")][SerializeField] private IntVariable playerLockVariable;
    [FoldoutGroup("Data")][SerializeField] private float textSpeed;
    [FoldoutGroup("Data")][SerializeField] private CanvasGroup displayGroup;
    [FoldoutGroup("Display")][SerializeField] private Image actorDisplayImage;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI actorNameText;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI displayText;

    public Dialogue currentDialogue;
    public int dialogueSegmentIndex;
    public DialogueSegment currentSegment;
    public string currentDialogueSegmentText;
    public bool atEndOfDialogueSegment;
    public bool active;

    private void Awake()
    {
        displayGroup.alpha = 0;
    }

    private void Update()
    {
        if (active && !atEndOfDialogueSegment && Input.GetKeyDown(interactionKey)) // skip to end of line
        {
            SkipToEndOfSegement();
        }
        else if (active && atEndOfDialogueSegment && Input.GetKeyDown(interactionKey)) // go to next text
        {
            SetupNextDialogueSegment();
        }
    }
    
    public void StartDialogue(Dialogue incomingDialogue)
    {
        if (!incomingDialogue.IsConditionMet() || active) return;
        displayText.text = "";
        playerLockVariable.Value++;
        currentDialogue = incomingDialogue;
        dialogueSegmentIndex = 0;
        active = true;
        displayGroup.alpha = 1;
        NextDialogueSegment();
    }

    private void SkipToEndOfSegement()
    {
        StopAllCoroutines();
        displayText.text = currentDialogueSegmentText;
        atEndOfDialogueSegment = true;
    }

    private void SetupNextDialogueSegment()
    {
        atEndOfDialogueSegment = false;
        dialogueSegmentIndex++;
        if (dialogueSegmentIndex >= currentDialogue.dialogueSegments.Length)
        {
            EndDialogue();
            currentSegment.OnFinishSegment.Invoke();
        }
        else
        {
            NextDialogueSegment();
            if (dialogueSegmentIndex != currentDialogue.dialogueSegments.Length - 1)
            {
                currentSegment.OnFinishSegment.Invoke();
            }
        }
    }
    
    private void NextDialogueSegment()
    {
        currentSegment = currentDialogue.dialogueSegments[dialogueSegmentIndex];
        if (currentSegment.IsConditionMet())
        {
            if (currentSegment.IsInstantSegment)
            {
                currentSegment.OnStartSegment.Invoke();
                currentSegment.OnFinishSegment.Invoke();
                SetupNextDialogueSegment();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ProcessText());
            }    
        }
        else SetupNextDialogueSegment();
    }
    
    IEnumerator ProcessText()
    {
        int charIndex = 0;

        currentSegment.OnStartSegment.Invoke();
        actorNameText.text = currentSegment.actor.actorName;
        actorDisplayImage.sprite = currentSegment.actor.actionIcon;
        
        displayText.text = "";
        currentDialogueSegmentText = currentSegment.speech;
        while (charIndex < currentDialogueSegmentText.Length)
        {
            displayText.text += currentDialogueSegmentText[charIndex];
            charIndex++;
            yield return new WaitForSeconds(textSpeed);
        }
        
        atEndOfDialogueSegment = true;
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        active = false;
        playerLockVariable.Value--;
        displayGroup.alpha = 0;
    }
}
