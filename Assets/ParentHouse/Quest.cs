using System;
using System.Collections.Generic;
using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    [CreateAssetMenu(fileName = "Quest", menuName = "Signatures/Quest")]
    public class Quest : Activity {
        [Title("Quest Settings")] public string QuestName;

        public QuestType QuestType;
        public QuestState CurrentState;

        [TextArea] public string QuestCompleteText;

        [Space(10)] [PropertyOrder(70)] public List<QuestTaskData> Tasks = new();

        [Space(10)] public QuestTask CurrentStep;

        public int TaskIndex;
        public bool AutoComplete;

        private StringVariable saveSlot;

        private void OnEnable() {
            LoadQuest();
        }

        [Button]
        [TabGroup("Functions", "Progress Functions")]
        public void StartQuest() {
            if (CurrentState is QuestState.Started or QuestState.Completed) return;
            TaskIndex = -1;
            CurrentState = QuestState.Started;
            StartNextTask();
            GameEvents.OnAcceptQuest.Invoke(this);
            GameEvents.OnUpdateQuests.Invoke();
            GameEvents.OnSendGenericMessage.Invoke($"Started Quest: {QuestName}");
        }

        public void TryCompleteTask(QuestTask incomingTask) {
            TryCompleteTask(incomingTask, 1);
        }

        public void TryCompleteTask(QuestTask incomingTask, int amount = 1) {
            if (CurrentState is not QuestState.Started or QuestState.ReadyToComplete) return;

            var task = Tasks[TaskIndex];

            if (task.taskTask != incomingTask) return;
            if (amount > task.numberOfRequiredHits - task.hits) amount = task.numberOfRequiredHits - task.hits;

            task.Hit(amount);
            Debug.Log(task.hits);
            GameEvents.OnMakeQuestProgress.Invoke(this);
            GameEvents.OnUpdateQuests.Invoke();

            if (task.IsTaskComplete())
                StartNextTask();
            else SaveQuest();
        }

        [Button]
        [TabGroup("Functions", "Progress Functions")]
        private void StartNextTask() {
            TaskIndex++;
            if (TaskIndex >= Tasks.Count && AutoComplete) {
                CompleteQuest();
                return;
            }

            if (TaskIndex >= Tasks.Count) TaskIndex--;

            CurrentStep = Tasks[TaskIndex].taskTask;
            var lastQuestTask = Tasks.Last();
            if (CurrentStep == lastQuestTask.taskTask &&
                lastQuestTask.hits == lastQuestTask.numberOfRequiredHits && !AutoComplete) {
                CurrentState = QuestState.ReadyToComplete;
                GameEvents.OnReadyToComplete.Invoke(this);
                GameEvents.OnUpdateQuests.Invoke();
            }

            SaveQuest();
        }

        public QuestTaskData GetCurrentQuestTask() {
            return TaskIndex >= Tasks.Count ? null : Tasks[TaskIndex];
        }

        [Button]
        [TabGroup("Functions", "Progress Functions")]
        public void CompleteQuest() {
            CurrentStep = null;
            if (QuestType == QuestType.Repeatable)
                ResetQuest();
            else CurrentState = QuestState.Completed;
            GameEvents.OnCompleteQuest.Invoke(this);
            GameEvents.OnUpdateQuests.Invoke();
            GameEvents.OnSendGenericMessage.Invoke($"Completed Quest: {QuestName}");
            SaveQuest();
        }

        [Button]
        [TabGroup("Functions", "Progress Functions")]
        private void ResetQuest() {
            foreach (var task in Tasks)
                task.hits = 0;
            TaskIndex = 0;
            CurrentStep = null;
            CurrentState = QuestState.NotStarted;
            GameEvents.OnUpdateQuests.Invoke();
            SaveQuest();
        }

        [Button]
        [TabGroup("Functions", "Data Functions")]
        private void SaveQuest() {
            // SaveLoad.SaveQuest(new QuestSaveData(this));
        }

        [Button]
        [TabGroup("Functions", "Data Functions")]
        private void LoadQuest() {
            // QuestSaveData loadedData = SaveLoad.LoadQuest(QuestName);
            // if (loadedData == null) return;
            //
            // TaskIndex = loadedData.SavedTaskIndex;
            // if (Tasks.Count < TaskIndex) CurrentStep = Tasks[TaskIndex].taskTask;
            //
            // int j = 0;
            // Tasks.ForEach(o =>
            // {
            //     o.hits = loadedData.SavedTaskHits[j];
            //     j++;
            // });
            // CurrentState = loadedData.SavedQuestState;
        }
    }

    [Serializable]
    public class QuestTaskData {
        public QuestTask taskTask;
        [FoldoutGroup("Display")] [TextArea] public string TaskDescription;
        [FoldoutGroup("Data")] public int numberOfRequiredHits = 1;
        [FoldoutGroup("Data")] public int hits;

        public void Hit(int amount = 1) {
            hits += amount;
        }

        public bool IsTaskComplete() {
            return hits >= numberOfRequiredHits;
        }
    }

    [Serializable]
    public class QuestSaveData {
        public string Name;
        public int SavedTaskIndex;
        public int[] SavedTaskHits;
        public QuestState SavedQuestState;

        public QuestSaveData(Quest quest) {
            Name = quest.QuestName;
            SavedTaskIndex = quest.TaskIndex;
            SavedTaskHits = quest.Tasks.Select(o => o.hits).ToArray();
            SavedQuestState = quest.CurrentState;
        }
    }
}