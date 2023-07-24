using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class SkillsDisplay : Window
{
    [SerializeField] private RectTransform skillsDisplayContainer;
    [SerializeField] private GameObject skillsDisplayListPrefab;
    [SerializeField] private List<Skill> skills = new();
    public List<SkillDisplayObject> displayObjects = new List<SkillDisplayObject>();

    private void OnEnable()
    {
        GameEvents.OnLevelUp += UpdateDisplays;
        PopulateSkillsDisplays();
    }

    private void UpdateDisplays(Sprite arg1, string arg2)
    {
        UpdateDisplays();
    }

    private void OnDisable()
    {
        GameEvents.OnLevelUp -= UpdateDisplays;
    }

    public override void Show()
    {
        base.Show();
        UpdateDisplays();
    }

    public void PopulateSkillsDisplays()
    {
        displayObjects.Clear();
        foreach (var skill in skills)
        {
            SpawnListObject(Instantiate(skillsDisplayListPrefab, skillsDisplayContainer),skill);
        }
    }

    public void UpdateDisplays()
    {
        foreach (var display in displayObjects)
        {
            display.obj.SetActive(display.skill.Level != 0);
        }
    }

    public void SpawnListObject(GameObject obj, Skill skill)
    {
        SkillDisplayObject newDisplayObject = new SkillDisplayObject();
        newDisplayObject.obj = obj;
        newDisplayObject.bar = obj.GetComponent<SkillDisplayBar>();
        newDisplayObject.bar.targetSkill = skill;
        newDisplayObject.skill = skill;
        newDisplayObject.bar.Setup();
        displayObjects.Add(newDisplayObject);
    }
    
#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        skills.Clear();
        var assets = AssetDatabase.FindAssets("t:Skill");
        
        foreach (var t in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( t );
            var asset = AssetDatabase.LoadAssetAtPath<Skill>( assetPath );
            skills.Add(asset);
        }
    }
#endif
}
[Serializable]
public class SkillDisplayObject
{
    public SkillDisplayBar bar;
    public Skill skill;
    public GameObject obj;
}
