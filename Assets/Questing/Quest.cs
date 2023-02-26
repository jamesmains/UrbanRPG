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
    [FoldoutGroup("Display")]public string QuestName;
    [FoldoutGroup("Display")][TextArea] public string QuestDescription;
    public List<QuestTaskData> questTasks = new();
    
    [FoldoutGroup("Status")]public QuestState QuestState;
    [FoldoutGroup("Status")]public QuestTaskSignature currentTaskSign;
    [FoldoutGroup("Status")]public int taskIndex;
    [FoldoutGroup("Status")]public bool isQuestStarted;
    [FoldoutGroup("Status")]public bool isQuestComplete;
    
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onAcceptQuest;
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onMakeQuestProgress;
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onCompleteQuest;
    
    private StringVariable saveSlot;

    [Button, FoldoutGroup("Progress Functions")]
    public void StartQuest()
    {
        if (isQuestStarted || isQuestComplete) return;
        isQuestStarted = true;
        taskIndex = -1;
        QuestState = QuestState.Started;
        onAcceptQuest.Raise(this);
        StartNextTask();
    }
    
    public int TryCompleteTask(QuestTaskSignature taskTaskSignature, int amount = 1)
    {
        if (isQuestComplete || !isQuestStarted) return 0;
        Debug.Log(amount);
        QuestTaskData task = questTasks[taskIndex];

        if (task.taskTaskSignature != currentTaskSign || taskTaskSignature != currentTaskSign)
        {
            return 0;
        }
        if (amount > (task.numberOfRequiredHits - task.hits))
        {
            amount = task.numberOfRequiredHits - task.hits;
        }
        
        task.Hit(amount);
        onMakeQuestProgress.Raise(this);
        
        if (task.IsTaskComplete())
        {
            StartNextTask();
        }
        else SaveQuest();

        return amount;
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

        currentTaskSign = questTasks[taskIndex].taskTaskSignature;
        SaveQuest();
    }

    [Button, FoldoutGroup("Progress Functions")]
    private void CompleteQuest()
    {
        isQuestComplete = true;
        currentTaskSign = null;
        QuestState = QuestState.Completed;
        onCompleteQuest.Raise(this);
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
        currentTaskSign = null;
        QuestState = QuestState.NotStarted;
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
        taskIndex = loadedData.SavedTaskIndex;
        if (questTasks.Count < taskIndex)
        {
            currentTaskSign = questTasks[taskIndex].taskTaskSignature;
        }
        
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
    public QuestTaskSignature taskTaskSignature;
    [FoldoutGroup("Display")]public string TaskName;
    [FoldoutGroup("Display")]public string TaskDescription;
    [FoldoutGroup("Data")]public int numberOfRequiredHits;
    [FoldoutGroup("Data")]public int hits;

    public void Hit(int amount = 1)
    {
        hits += amount;
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
    public int SavedTaskIndex;
    public int[] SavedTaskHits;
    public bool SavedStartedState;
    public bool SavedCompletedState;

    public QuestSaveData(Quest quest)
    {
        Name = quest.QuestName;
        SavedTaskIndex = quest.taskIndex;
        SavedTaskHits = quest.questTasks.Select(o => o.hits).ToArray();
        SavedStartedState = quest.isQuestStarted;
        SavedCompletedState = quest.isQuestComplete;
    }
}