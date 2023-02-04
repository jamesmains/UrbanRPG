using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey; // todo replace with scriptable objects for key rebinding
    [SerializeField] private IntVariable playerLockVariable;
    [SerializeField] private float textSpeed;
    [SerializeField] private Image actorDisplayImage;
    [SerializeField] private TextMeshProUGUI actorNameText;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private CanvasGroup displayGroup;

    public Dialogue currentDialogue;
    public int dialogueSegmentIndex;
    public string currentDialogueSegmentText;
    public bool atEndOfDialogueSegment;
    public bool active;

    private void Awake()
    {
        displayGroup.alpha = 0;
    }

    public void StartDialogue(Dialogue incomingDialogue)
    {
        displayText.text = "";
        playerLockVariable.Value++;
        currentDialogue = incomingDialogue;
        dialogueSegmentIndex = 0;
        active = true;
        displayGroup.alpha = 1;

        NextDialogueSegment();
    }

    IEnumerator ProcessText()
    {
        atEndOfDialogueSegment = false;
        
        int charIndex = 0;

        actorNameText.text = currentDialogue.dialogueSegments[dialogueSegmentIndex].actor.actorName;
        actorDisplayImage.sprite = currentDialogue.dialogueSegments[dialogueSegmentIndex].actor.actionIcon;
        
        displayText.text = "";
        currentDialogueSegmentText = currentDialogue.dialogueSegments[dialogueSegmentIndex].speech;
        while (charIndex < currentDialogueSegmentText.Length)
        {
            
            displayText.text += currentDialogueSegmentText[charIndex];
            charIndex++;
            yield return new WaitForSeconds(textSpeed);
        }
        atEndOfDialogueSegment = true;
    }

    private void Update()
    {
        if (active && !atEndOfDialogueSegment && Input.GetKeyDown(interactionKey)) // skip to end of line
        {
            StopAllCoroutines();
            displayText.text = currentDialogueSegmentText;
            atEndOfDialogueSegment = true;
        }
        else if (active && atEndOfDialogueSegment && Input.GetKeyDown(interactionKey)) // go to next text
        {
            dialogueSegmentIndex++;
            if (dialogueSegmentIndex >= currentDialogue.dialogueSegments.Length)
            {
                EndDialogue();
            }
            else NextDialogueSegment();
        }
    }

    private void NextDialogueSegment()
    {
        StartCoroutine(ProcessText());
    }

    private void EndDialogue()
    {
        active = false;
        playerLockVariable.Value--;
        displayGroup.alpha = 0;
    }
}
