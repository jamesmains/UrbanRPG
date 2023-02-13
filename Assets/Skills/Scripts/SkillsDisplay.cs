using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform skillsDisplayContainer;
    [SerializeField] private GameObject skillsDisplayListPrefab;
    [SerializeField] private Skill[] skills;
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
}
[Serializable]
public class SkillDisplayObject
{
    public SkillDisplayBar bar;
    public Skill skill;
}
