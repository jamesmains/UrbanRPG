using System.Collections.Generic;
using System.Linq;
using ParentHouse.UI;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ParentHouse {
    public class QuestsWindow : WindowPanel {
        [FoldoutGroup("Display")] [SerializeField]
        private GameObject questDisplayObject;

        [FoldoutGroup("Display")] [SerializeField]
        private RectTransform questDisplayContainer;

        public List<Quest> quests = new();

        public override void Show() {
            base.Show();
            UpdateQuests();
        }

        public override void Hide() {
            base.Hide();
            //foreach (var questDisplay in questDisplays) questDisplay.CloseFoldout();
        }

        private void PopulateQuests() {
            foreach (var quest in quests) {
                //var QuestDisplay = Instantiate(questDisplayObject, questDisplayContainer).GetComponent<QuestDisplay>();
                //QuestDisplay.Setup(quest);
                //questDisplays.Add(QuestDisplay);
            }
        }

        private void UpdateQuests() {
            //foreach (var questDisplay in questDisplays) questDisplay.UpdateDisplay();
        }

        public bool HasDreamQuest() // We don't want the player to be able to get multiple dream quests atm
        {
            return quests.Any(q =>
                q.QuestType == QuestType.Dream &&
                q.CurrentState is QuestState.Started or QuestState.ReadyToComplete);
        }

#if UNITY_EDITOR
        [Button]
        public void FindAssetsByType() {
            quests.Clear();
            var assets = AssetDatabase.FindAssets("t:Quest");

            foreach (var t in assets) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<Quest>(assetPath);
                quests.Add(asset);
            }
        }

        [Button]
        private void ClearDisplays() {
            questDisplayContainer.DestroyChildrenInEditor();
            quests.Clear();
        }
#endif
    }
}