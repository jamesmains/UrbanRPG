using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class SkillDisplayBar : DisplayBar
{
    [FoldoutGroup("Display")] [SerializeField] private TextMeshProUGUI levelText;
    [FoldoutGroup("Data")] public Skill targetSkill;
    protected void Awake()
    {
        Setup();
    }

    private void Update()
    {
        UpdateSkillDisplayBar();
    }

    public void Setup()
    {
        if(nameText!=null) nameText.text = targetSkill.Name;
        if(descriptionText!=null) descriptionText.text = targetSkill.Description;
        if (iconImage != null) iconImage.sprite = targetSkill.Icon;
        UpdateSkillDisplayBar();
    }

    public void UpdateSkillDisplayBar()
    {
        
        if (levelText != null) levelText.text = $"Lvl {targetSkill.Level.ToString()}";
        UpdateBar((float)targetSkill.CurrentExp / (float)targetSkill.ExpRequiredForLevelUp);
    }
}
