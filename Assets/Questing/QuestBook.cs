using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class QuestBook : MonoBehaviour
{
    [SerializeField] private GameObject questDisplayObject;
    [SerializeField] private RectTransform questDisplayObjectContainer;
    public List<Quest> questList = new();
    public BoolVariable HasDreamQuestVariable;

    private void Awake()
    {
        HasDreamQuestVariable.Value = HasDreamQuestVariable;
    }

    private void OnEnable()
    {
        PopulateQuestDisplay();
    }

    public void PopulateQuestDisplay()
    {
        var oldItems = questDisplayObjectContainer?.GetComponentsInChildren<Transform>();
        foreach (var listObject in oldItems)
        {
            if (listObject != questDisplayObjectContainer)
            {
                Destroy(listObject.gameObject);
            }
        }
        foreach (Quest quest in questList)
        {
            if (quest.QuestState != QuestState.Completed && quest.QuestState != QuestState.NotStarted)
            {
                QuestTaskData questTask = quest.GetCurrentQuestTask();
                string qTaskProgress = questTask.numberOfRequiredHits > 1 ? $"{questTask.hits} / {questTask.numberOfRequiredHits}" : "";
                
                Instantiate(questDisplayObject,questDisplayObjectContainer).GetComponent<QuestDisplay>().Setup(
                    quest.QuestName,
                    quest.QuestDescription,
                    questTask.TaskDescription,
                    qTaskProgress
                    );
            }
        }
    }

    public bool HasDreamQuest()
    {
        return questList.Any(q =>
            q.QuestType == QuestType.Dream &&
            (q.QuestState == QuestState.Started || q.QuestState == QuestState.ReadyToComplete));
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
