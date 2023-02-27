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
    [FoldoutGroup("Details")]public string QuestName;
    [FoldoutGroup("Details")]public QuestType QuestType;
    [FoldoutGroup("Details")][TextArea] public string QuestDescription;
    public List<QuestTaskData> questTasks = new();
    
    [FoldoutGroup("Status")]public QuestState QuestState;
    [FoldoutGroup("Status")]public QuestTaskSignature currentTaskSign;
    [FoldoutGroup("Status")]public int taskIndex;
    [FoldoutGroup("Status")]public bool autoComplete;
    
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onAcceptQuest;
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onMakeQuestProgress;
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onReadyToComplete;
    [SerializeField] [FoldoutGroup("Events")]
    private QuestGameEvent onCompleteQuest;
    
    private StringVariable saveSlot;

    [Button, FoldoutGroup("Progress Functions")]
    public void StartQuest()
    {
        if (QuestState == QuestState.Started || QuestState == QuestState.Completed) return;
        taskIndex = -1;
        QuestState = QuestState.Started;
        onAcceptQuest.Raise(this);
        StartNextTask();
    }
    
    public int TryCompleteTask(QuestTaskSignature taskTaskSignature, int amount = 1)
    {
        if (QuestState != QuestState.Started || QuestState == QuestState.ReadyToComplete) return 0;
        
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
        if (taskIndex >= questTasks.Count && autoComplete)
        {
            CompleteQuest();
            return;
        }
        else if(taskIndex >= questTasks.Count) taskIndex--;
        currentTaskSign = questTasks[taskIndex].taskTaskSignature;
        var lastQuestTask = questTasks.Last();
        if (currentTaskSign == lastQuestTask.taskTaskSignature &&
            lastQuestTask.hits == lastQuestTask.numberOfRequiredHits && !autoComplete)
        {
            QuestState = QuestState.ReadyToComplete;
            onReadyToComplete.Raise(this);
        }
        SaveQuest();
    }

    [Button, FoldoutGroup("Progress Functions")]
    public void CompleteQuest()
    {
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
        QuestState = loadedData.SavedQuestState;
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
    public QuestState SavedQuestState;

    public QuestSaveData(Quest quest)
    {
        Name = quest.QuestName;
        SavedTaskIndex = quest.taskIndex;
        SavedTaskHits = quest.questTasks.Select(o => o.hits).ToArray();
        SavedQuestState = quest.QuestState;
    }
}