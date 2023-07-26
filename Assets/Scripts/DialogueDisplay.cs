using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : Window
{
    [FoldoutGroup("Data")][SerializeField] private float textSpeed;
    [FoldoutGroup("Display")][SerializeField] private Image actorDisplayImage;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI actorNameText;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI displayText;

    [SerializeField,FoldoutGroup("Debug"),ReadOnly] private Dialogue currentDialogue;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly] private int dialogueSegmentIndex;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly] private string currentDialogueSegmentText;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly] private bool atEndOfDialogueSegment;
    
    private DialogueSegment currentSegment;

    private void OnEnable()
    {
        GameEvents.StartDialogueEvent += StartDialogue;
        GameEvents.OnInteractButtonDown += delegate
        {
            switch (isActive)
            {
                case true when !atEndOfDialogueSegment:
                    SkipToEndOfSegement();
                    break;
                case true when atEndOfDialogueSegment:
                    SetupNextDialogueSegment();
                    break;
            }
        };
    }

    private void OnDisable()
    {
        GameEvents.StartDialogueEvent -= StartDialogue;
        GameEvents.OnInteractButtonDown -= delegate
        {
            switch (isActive)
            {
                case true when !atEndOfDialogueSegment:
                    SkipToEndOfSegement();
                    break;
                case true when atEndOfDialogueSegment:
                    SetupNextDialogueSegment();
                    break;
            }
        };
    }
    
    public void StartDialogue(Dialogue incomingDialogue)
    {
        if (!incomingDialogue.IsConditionMet()) return;
        Show();
        displayText.text = "";
        Global.PlayerLock++;
        currentDialogue = incomingDialogue;
        dialogueSegmentIndex = 0;
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
        if (dialogueSegmentIndex >= currentDialogue.DialogueSegments.Count)
        {
            EndDialogue();
            if (currentSegment.IsConditionMet())
            {
                currentSegment?.OnFinishSegment?.Invoke();
            }
        }
        else
        {
            NextDialogueSegment();
            if (dialogueSegmentIndex != currentDialogue.DialogueSegments.Count - 1)
            {
                if (currentSegment.IsConditionMet())
                {
                    currentSegment?.OnFinishSegment?.Invoke();
                }
            }
        }
    }
    
    private void NextDialogueSegment()
    {
        currentSegment = currentDialogue.DialogueSegments[dialogueSegmentIndex];
        if (currentSegment.IsConditionMet())
        {
            if (currentSegment.IsInstantSegment)
            {
                currentSegment.OnStartSegment?.Invoke();
                currentSegment.OnFinishSegment?.Invoke();
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

        currentSegment?.OnStartSegment?.Invoke();
        if (currentSegment != null)
        {
            bool hasActor = currentSegment.actor != null;
            actorNameText.transform.parent.gameObject.SetActive(hasActor);
            actorDisplayImage.enabled = hasActor;
            if (hasActor)
            {
                actorNameText.text = currentSegment.actor.actorName;
                actorDisplayImage.sprite = currentSegment.actor.actionIcon;    
            }

            displayText.text = "";
            currentDialogueSegmentText = currentSegment.speech;
        }

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
        Hide();
        Global.PlayerLock--;
        currentDialogue.EndDialogue();
    }
}
