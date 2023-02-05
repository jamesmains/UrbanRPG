using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    [SerializeField] private string QuestName;
    [SerializeField][TextArea] private string QuestDescription;
    [SerializeField] private List<QuestTaskData> questTasks = new();

    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onAcceptQuest;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onMakeQuestProgress;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onCompleteQuest;

    private string currentTaskName;
    private int taskIndex;
    public bool isQuestStarted;
    public bool isQuestComplete;

    public void StartQuest()
    {
        if (isQuestStarted) return;
        isQuestStarted = true;
        taskIndex = -1;
        onAcceptQuest.Raise();
        StartNextTask();
    }
    
    public void TryCompleteTask(QuestSignature taskSignature)
    {
        if (isQuestComplete || !isQuestStarted) return;
        
        QuestTaskData task = questTasks.Find(o => o.TaskSignature == taskSignature);
        if (task == null)
        {
            Debug.LogError($"Quest: Unable to find task {taskSignature}");
            return;
        }

        if (task.TaskName != currentTaskName)
        {
            return;
        }
        
        task.Hit();
        onMakeQuestProgress.Raise();
        if (task.IsTaskComplete())
        {
            StartNextTask();
        }
    }

    private void StartNextTask()
    {
        taskIndex++;
        if (taskIndex >= questTasks.Count)
        {
            CompleteQuest();
            return;
        }
        currentTaskName = questTasks[taskIndex].TaskName;
    }

    private void CompleteQuest()
    {
        isQuestComplete = true;
        onCompleteQuest.Raise();
    }

    [Button]
    private void ResetQuest()
    {
        foreach (var task in questTasks)
            task.hits = 0;
        isQuestComplete = false;
        isQuestStarted = false;
    }
}

[Serializable]
public class QuestTaskData
{
    public QuestSignature TaskSignature;
    public string TaskName;
    public string TaskDescription;
    public int numberOfRequiredHits;
    public int hits;

    public void Hit()
    {
        hits++;
    }
    
    public bool IsTaskComplete()
    {
        return hits >= numberOfRequiredHits;
    }
}