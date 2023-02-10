using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// TODO need to rework this
public class QuestObjectToggle : MonoBehaviour
{
    [SerializeField] private Quest targetQuest;
    [SerializeField] private bool checkOnAwake;
    [SerializeField] private UnityEvent onQuestCompleted;
    
    private void Awake()
    {
        if (checkOnAwake)
        {
            CheckQuest();
        }
    }

    public void CheckQuest()
    {
        if (targetQuest.isQuestComplete)
        {
            onQuestCompleted.Invoke();
            return;
        }
    }
}
