using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class NeedsWindow : Window
{
    [SerializeField] private RectTransform needsDisplayContainer;
    [SerializeField] private GameObject needsDisplayListPrefab;
    [SerializeField] private List<Need> needs = new();
    public List<NeedDisplayObject> displayObjects = new List<NeedDisplayObject>();
    
    private void OnEnable()
    {
        // GameEvents.OnLevelUp += UpdateDisplays;
        PopulateSkillsDisplays();
    }

    private void OnDisable()
    {
        // GameEvents.OnLevelUp -= UpdateDisplays;
    }
    
    public override void Show()
    {
        base.Show();
    }

    public void PopulateSkillsDisplays()
    {
        displayObjects.Clear();
        foreach (var need in needs)
        {
            SpawnListObject(Instantiate(needsDisplayListPrefab, needsDisplayContainer),need);
        }
    }

    public void SpawnListObject(GameObject obj, Need need)
    {
        NeedDisplayObject newDisplayObject = new NeedDisplayObject();
        newDisplayObject.obj = obj;
        newDisplayObject.bar = obj.GetComponent<NeedDisplayBar>();
        newDisplayObject.bar.targetNeed = need;
        newDisplayObject.need = need;
        newDisplayObject.bar.Setup();
        displayObjects.Add(newDisplayObject);
    }
    
#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        needs.Clear();
        var assets = AssetDatabase.FindAssets("t:Need");
        
        foreach (var t in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( t );
            var asset = AssetDatabase.LoadAssetAtPath<Need>( assetPath );
            needs.Add(asset);
        }
    }
#endif
}
[Serializable]
public class NeedDisplayObject
{
    public NeedDisplayBar bar;
    public Need need;
    public GameObject obj;
}
