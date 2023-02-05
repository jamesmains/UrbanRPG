using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class QuestBook : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new();

    public void TryCompleteQuestTask(QuestSignature taskSignature)
    {
        foreach (Quest quest in questList)
        {
            quest.TryCompleteTask(taskSignature);
        }
    }
}
