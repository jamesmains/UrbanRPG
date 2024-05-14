using System.Collections.Generic;
using System.Linq;
using ParentHouse.UI;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    public class QuestBook : WindowPanel {
        [SerializeField] private GameObject questDisplayObject;
        [SerializeField] private RectTransform questDisplayObjectContainer;
        public List<Quest> questList = new();

        public void PopulateQuestDisplay() {
            foreach (Transform child in questDisplayObjectContainer.transform) Destroy(child.gameObject);

            foreach (var quest in questList) {
                if (quest.CurrentState == QuestState.Completed || quest.CurrentState == QuestState.NotStarted) continue;

                var questTask = quest.GetCurrentQuestTask();
                var qTaskProgress = questTask.numberOfRequiredHits > 1
                    ? $"{questTask.hits} / {questTask.numberOfRequiredHits}"
                    : "";

                // Instantiate(questDisplayObject,questDisplayObjectContainer).GetComponent<QuestDisplay>().Setup(
                //     quest.QuestName,
                //     quest.QuestDescription,
                //     questTask.TaskDescription,
                //     qTaskProgress
                // );
            }
        }

        public bool HasDreamQuest() {
            return questList.Any(q =>
                q.QuestType == QuestType.Dream &&
                (q.CurrentState == QuestState.Started || q.CurrentState == QuestState.ReadyToComplete));
        }

#if UNITY_EDITOR
        [Button]
        public void FindAssetsByType() {
            questList.Clear();
            var quests = AssetDatabase.FindAssets("t:Quest");

            for (var i = 0; i < quests.Length; i++) {
                var assetPath = AssetDatabase.GUIDToAssetPath(quests[i]);
                var asset = AssetDatabase.LoadAssetAtPath<Quest>(assetPath);
                questList.Add(asset);
            }
        }
#endif
    }
}