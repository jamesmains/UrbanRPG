using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestObjectToggle : MonoBehaviour
{
    [SerializeField] private Quest targetQuest;
    [SerializeField] private UnityEvent onQuestNotStarted;
    [SerializeField] private UnityEvent onQuestStarted;
    [SerializeField] private UnityEvent onQuestCompleted;

    private bool questStartedStatus, questCompletedStatus;
    
    private void Awake()
    {
        questStartedStatus = targetQuest.isQuestStarted;
        questCompletedStatus = targetQuest.isQuestComplete;
        if (!questStartedStatus)
        {
            onQuestNotStarted.Invoke();
        }
    }

    public void CheckQuest()
    {
        if (targetQuest.isQuestStarted && !questStartedStatus)
        {
            onQuestStarted.Invoke();
            return;
        }
        if (targetQuest.isQuestComplete && !questCompletedStatus)
        {
            onQuestCompleted.Invoke();
            return;
        }
    }
}
