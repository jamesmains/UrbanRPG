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

    private void Start()
    {
        displayObjects.Clear();
        UpdateSkillsDisplay();
    }

    public void UpdateSkillsDisplay()
    {
        foreach (var skill in skills)
        {
            if(skill.Level == 0) continue;
            var existingObject = displayObjects.FindAll(o => o.skill == skill);
            if(existingObject.Count == 0)
                SpawnListObject(Instantiate(skillsDisplayListPrefab, skillsDisplayContainer),skill);
        }
    }

    public void SpawnListObject(GameObject obj, Skill skill)
    {
        SkillDisplayObject newDisplayObject = new SkillDisplayObject();
        var bar = obj.GetComponent<SkillDisplayBar>();
        newDisplayObject.bar = bar;
        bar.targetSkill = skill;
        newDisplayObject.skill = skill;
        displayObjects.Add(newDisplayObject);
        bar.Setup();
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
}
