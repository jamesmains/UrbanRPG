using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class QuestBook : Window
{
    [SerializeField] private GameObject questDisplayObject;
    [SerializeField] private RectTransform questDisplayObjectContainer;
    public List<Quest> questList = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        PopulateQuestDisplay();
    }

    public void PopulateQuestDisplay()
    {
        foreach (Transform child in questDisplayObjectContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Quest quest in questList)
        {
            if (quest.CurrentState == QuestState.Completed || quest.CurrentState == QuestState.NotStarted) continue;
            
            QuestTaskData questTask = quest.GetCurrentQuestTask();
            string qTaskProgress = questTask.numberOfRequiredHits > 1 ? $"{questTask.hits} / {questTask.numberOfRequiredHits}" : "";
                
            // Instantiate(questDisplayObject,questDisplayObjectContainer).GetComponent<QuestDisplay>().Setup(
            //     quest.QuestName,
            //     quest.QuestDescription,
            //     questTask.TaskDescription,
            //     qTaskProgress
            // );
        }
    }

    public bool HasDreamQuest()
    {
        return questList.Any(q =>
            q.QuestType == QuestType.Dream &&
            (q.CurrentState == QuestState.Started || q.CurrentState == QuestState.ReadyToComplete));
    }
    
#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        questList.Clear();
        var quests = AssetDatabase.FindAssets("t:Quest");
        
        for (int i = 0; i < quests.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( quests[i] );
            var asset = AssetDatabase.LoadAssetAtPath<Quest>( assetPath );
            questList.Add(asset);
        }
    }
#endif
}
