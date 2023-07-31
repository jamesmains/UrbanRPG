using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestsWindow : Window
{
    [FoldoutGroup("Display")][SerializeField] private GameObject questDisplayObject;
    [FoldoutGroup("Display")][SerializeField] private RectTransform questDisplayContainer;
    [FoldoutGroup("Display")][SerializeField] private List<QuestDisplay> questDisplays;
    public List<Quest> quests = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        PopulateQuests();
    }

    public override void Show()
    {
        base.Show();
        UpdateQuests();
    }

    public override void Hide()
    {
        base.Hide();
        foreach (var questDisplay in questDisplays)
        {
            questDisplay.CloseFoldout();
        }
    }

    private void PopulateQuests()
    {
        foreach (Quest quest in quests)
        {
            var QuestDisplay = Instantiate(questDisplayObject,questDisplayContainer).GetComponent<QuestDisplay>();
            QuestDisplay.Setup(quest);
            questDisplays.Add(QuestDisplay);
        }
    }

    private void UpdateQuests()
    {
        foreach (var questDisplay in questDisplays)
        {
            questDisplay.UpdateDisplay();
        }
    }

    public bool HasDreamQuest() // We don't want the player to be able to get multiple dream quests atm
    {
        return quests.Any(q =>
            q.QuestType == QuestType.Dream &&
            q.CurrentState is QuestState.Started or QuestState.ReadyToComplete);
    }

#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        quests.Clear();
        var assets = AssetDatabase.FindAssets("t:Quest");
        
        foreach (var t in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( t );
            var asset = AssetDatabase.LoadAssetAtPath<Quest>( assetPath );
            quests.Add(asset);
        }
    }    [Button]
    private void ClearDisplays()
    {
        questDisplayContainer.DestroyChildrenInEditor();
        quests.Clear();
    }
#endif
}
