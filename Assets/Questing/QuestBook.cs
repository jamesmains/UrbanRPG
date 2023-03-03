using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class QuestBook : MonoBehaviour
{
    public List<Quest> questList = new();
    public BoolVariable HasDreamQuestVariable;

    private void Awake()
    {
        HasDreamQuestVariable.Value = HasDreamQuestVariable;
    }

    public void TryCompleteQuestTask(QuestTaskSignature taskTaskSignature)
    {
        foreach (Quest quest in questList)
        {
            quest.TryCompleteTask(taskTaskSignature);
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
