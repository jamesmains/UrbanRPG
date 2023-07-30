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
    [SerializeField] private float openSpeed;
    
    private Quest heldQuest;
    private bool isOpen;
    private bool hidden;
    private Vector2 openSize;
    private Vector2 closedSize;
    private static QuestDisplay openedDisplay;
    private string heldQuestDescription;

    private void OnEnable()
    {
        GameEvents.OnUpdateQuests += UpdateDisplay;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateQuests -= UpdateDisplay;
    }

    private void Update()
    {
        
        if (isOpen && displayRect.sizeDelta != openSize)
        {
            var size = displayRect.sizeDelta;
            size.y = Mathf.Lerp(size.y, openSize.y, openSpeed * Time.deltaTime);
            displayRect.sizeDelta = size;
        }
        else if (!isOpen && displayRect.sizeDelta != closedSize)
        {
            var size = displayRect.sizeDelta;
            size.y = Mathf.Lerp(size.y, closedSize.y, openSpeed * Time.deltaTime);
            displayRect.sizeDelta = size;
        }
    }

    public void Setup(Quest quest)
    {
        heldQuest = quest;
        closedSize = new Vector2(displayRect.rect.width, 50);
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        Show();
        
        if(openedDisplay != null) openedDisplay.CloseFoldout();
        var currentQuestState = heldQuest.CurrentState;
        checkMark.SetActive(currentQuestState == QuestState.Completed);
        hidden = currentQuestState == QuestState.NotStarted;
        
        QuestTaskData questTask = heldQuest.GetCurrentQuestTask();
        if (questTask != null)
        {
            string qTaskProgress = questTask.numberOfRequiredHits > 1 ? $"\n{questTask.hits} / {questTask.numberOfRequiredHits}" : "";
            heldQuestDescription = heldQuest.GetCurrentQuestTask().TaskDescription + qTaskProgress;
        }
        else
        {
            heldQuestDescription = heldQuest.QuestCompleteText;
        }
        
        heldQuestDescription = hidden ? "I could have sworn there was something I was supposed to do..." : heldQuestDescription;
        questNameText.text = hidden ? "???" : heldQuest.QuestName;
    }

    private void SetOpenSize()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(foldoutRect);
        openSize = new Vector2(displayRect.rect.height, 58 + foldoutRect.sizeDelta.y);
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
        questTaskDescriptionText.text = heldQuestDescription;
        SetOpenSize();
        isOpen = true;
    }

    public void CloseFoldout()
    {
        questTaskDescriptionText.text = "";
        isOpen = false;
    }
    
}
