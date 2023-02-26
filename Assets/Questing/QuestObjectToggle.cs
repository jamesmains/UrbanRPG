using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// TODO need to rework this
public class QuestObjectToggle : MonoBehaviour
{
    [SerializeField] private QuestRequirement[] questRequirements;
    [SerializeField] private bool checkOnAwake;
    [SerializeField] private bool runIfAny;
    [SerializeField] private UnityEvent onMeetsRequirements;
    [SerializeField] private UnityEvent onFailsRequirements;
    
    private void Awake()
    {
        if (checkOnAwake)
        {
            CheckQuest();
        }
    }

    public void CheckQuest()
    {
        bool canDo = !runIfAny;
        if (questRequirements.Length <= 0) return;
        foreach (QuestRequirement questRequirement in questRequirements)
        {
            if (!questRequirement.MeetsRequirements() && !runIfAny)
            {
                canDo = false;
                break;
            }
            else if (questRequirement.MeetsRequirements() && runIfAny)
            {
                canDo = true;
                break;
            }
        }

        if (canDo)
        {
            onMeetsRequirements.Invoke();    
        }
        else
        {
            onFailsRequirements.Invoke();
        }
    }
}

[Serializable]
public class QuestRequirement
{
    [SerializeField] private Quest quest;
    [SerializeField] private QuestState requiredState;

    public bool MeetsRequirements()
    {
        switch (requiredState)
        {
            case QuestState.Started when !quest.isQuestComplete && quest.isQuestStarted:
            case QuestState.Completed when quest.isQuestComplete:
            case QuestState.NotStarted when !quest.isQuestComplete && !quest.isQuestStarted:
                return true;
            default:
                return false;
        }
    }
}
