using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public string QuestName;
    [TextArea] public string QuestDescription;
    public List<QuestTaskData> questTasks = new();

    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onAcceptQuest;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onMakeQuestProgress;
    [SerializeField] [FoldoutGroup("Events")]
    private GameEvent onCompleteQuest;

    public string currentTaskName;
    public int taskIndex;
    public bool isQuestStarted;
    public bool isQuestComplete;
    private StringVariable saveSlot;

    [Button, FoldoutGroup("Progress Functions")]
    public void StartQuest()
    {
        if (isQuestStarted || isQuestComplete) return;
        isQuestStarted = true;
        taskIndex = -1;
        onAcceptQuest.Raise();
        StartNextTask();
    }
    
    public void TryCompleteTask(QuestSignature taskSignature)
    {
        if (isQuestComplete || !isQuestStarted) return;

        QuestTaskData task = questTasks[taskIndex];

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

    [Button, FoldoutGroup("Progress Functions")]
    private void StartNextTask()
    {
        taskIndex++;
        if (taskIndex >= questTasks.Count)
        {
            CompleteQuest();
            return;
        }
        currentTaskName = questTasks[taskIndex].TaskName;
        SaveQuest();
    }

    [Button, FoldoutGroup("Progress Functions")]
    private void CompleteQuest()
    {
        isQuestComplete = true;
        onCompleteQuest.Raise();
        SaveQuest();
    }

    [Button, FoldoutGroup("Progress Functions")]
    private void ResetQuest()
    {
        foreach (var task in questTasks)
            task.hits = 0;
        isQuestComplete = false;
        isQuestStarted = false;
        taskIndex = 0;
        currentTaskName = "";
    }

    [Button, FoldoutGroup("Data Functions")]
    private void SaveQuest()
    {
        SaveLoad.SaveQuest(new QuestSaveData(this));
    }

    [Button, FoldoutGroup("Data Functions")]
    private void LoadQuest()
    {
        QuestSaveData loadedData = SaveLoad.LoadQuest(QuestName);
        if (loadedData == null)
        {
            return;
        }
        currentTaskName = loadedData.SavedTaskName;
        taskIndex = loadedData.SavedTaskIndex;
        int j = 0;
        questTasks.ForEach(o =>
        {
            o.hits = loadedData.SavedTaskHits[j];
            j++;
        });
        isQuestStarted = loadedData.SavedStartedState;
        isQuestComplete = loadedData.SavedCompletedState;
    }

    private void OnEnable()
    {
        LoadQuest();
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

[Serializable]
public class QuestSaveData
{
    public string Name;
    public string SavedTaskName;
    public int SavedTaskIndex;
    public int[] SavedTaskHits;
    public bool SavedStartedState;
    public bool SavedCompletedState;

    public QuestSaveData(Quest quest)
    {
        Name = quest.QuestName;
        SavedTaskIndex = quest.taskIndex;
        SavedTaskName = quest.currentTaskName;
        SavedTaskHits = quest.questTasks.Select(o => o.hits).ToArray();
        SavedStartedState = quest.isQuestStarted;
        SavedCompletedState = quest.isQuestComplete;
    }
}