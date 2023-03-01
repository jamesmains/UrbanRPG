using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class QuestBook : MonoBehaviour
{
    public List<Quest> questList = new();

    public void TryCompleteQuestTask(QuestTaskSignature taskTaskSignature)
    {
        foreach (Quest quest in questList)
        {
            quest.TryCompleteTask(taskTaskSignature);
        }
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
