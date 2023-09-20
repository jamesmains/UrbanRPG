using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDisplay : FoldoutDisplay
{
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questTaskDescriptionText;
    [SerializeField] private GameObject checkMark;
    private Quest heldQuest;
    private bool hidden;

    private string heldQuestDescription;

    public void Setup(Quest quest)
    {
        heldQuest = quest;
        closedSize = new Vector2(displayRect.rect.width, 50);
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if(openedDisplay != null) openedDisplay.CloseFoldout();
        var currentQuestState = heldQuest.CurrentState;
        checkMark.SetActive(currentQuestState == QuestState.Completed);
        hidden = currentQuestState == QuestState.NotStarted;
        
        QuestTaskData questTask = heldQuest.GetCurrentQuestTask();
        if (questTask != null && currentQuestState != QuestState.Completed)
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

    public override void OpenFoldout()
    {
        questTaskDescriptionText.text = heldQuestDescription;
        base.OpenFoldout();
    }

    public override void CloseFoldout()
    {
        questTaskDescriptionText.text = "";
        base.CloseFoldout();
    }
}
