using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : Window
{
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questTaskDescriptionText;
    [SerializeField] private RectTransform displayRect;
    [SerializeField] private RectTransform foldoutRect;
    [SerializeField] private GameObject checkMark;
    // [SerializeField] private VerticalLayoutGroup foldoutLayoutGroup;
    
    private Quest heldQuest;
    private bool isOpen;
    private bool hidden;
    private static QuestDisplay openedDisplay;

    private void OnEnable()
    {
        GameEvents.OnUpdateQuests += UpdateDisplay;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateQuests -= UpdateDisplay;
    }

    public void Setup(Quest quest)
    {
        heldQuest = quest;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        Show();
        LayoutRebuilder.ForceRebuildLayoutImmediate(foldoutRect);
        
        if(openedDisplay != null) openedDisplay.CloseFoldout();
        var currentQuestState = heldQuest.CurrentState;
        checkMark.SetActive(currentQuestState == QuestState.Completed);
        hidden = currentQuestState == QuestState.NotStarted;
        
        QuestTaskData questTask = heldQuest.GetCurrentQuestTask();
        if (questTask != null)
        {
            string qTaskProgress = questTask.numberOfRequiredHits > 1 ? $"\n{questTask.hits} / {questTask.numberOfRequiredHits}" : "";
            questTaskDescriptionText.text = heldQuest.GetCurrentQuestTask().TaskDescription + qTaskProgress;
        }
        else
        {
            questTaskDescriptionText.text = heldQuest.QuestCompleteText;
        }
        
        questTaskDescriptionText.text = hidden ? "I could have sworn there was something I was supposed to do..." : questTaskDescriptionText.text;
        questNameText.text = hidden ? "???" : heldQuest.QuestName;
        
        
        foldoutRect.gameObject.SetActive(false);
    }

    public void ToggleDisplay()
    {
        if(openedDisplay == null)
            openedDisplay = this;
        
        if(openedDisplay != null && openedDisplay != this)
        {
            openedDisplay.CloseFoldout();
        }
        
        openedDisplay = this;
        
        if (isOpen)
        {
            CloseFoldout();
        }
        else
        {
            OpenFoldout();
        }
    }

    private void OpenFoldout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(foldoutRect);
        Vector2 rectSize = new Vector2(displayRect.sizeDelta.x, 58 + foldoutRect.sizeDelta.y);// Todo: maybe replace 58 with proper padding?
        foldoutRect.gameObject.SetActive(true);
        displayRect.sizeDelta = rectSize;
        isOpen = true;
    }

    public void CloseFoldout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(foldoutRect);
        Vector2 rectSize = new Vector2(displayRect.sizeDelta.x, 50);
        foldoutRect.gameObject.SetActive(false);
        displayRect.sizeDelta = rectSize;
        isOpen = false;
    }
    
}
