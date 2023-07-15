using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedDisplayBar : DisplayBar
{
    [FoldoutGroup("Data")] [SerializeField] private Need targetNeed;

    protected void Awake()
    {
        if(nameText!=null) nameText.text = targetNeed.Name;
        if(descriptionText!=null) descriptionText.text = targetNeed.Description;
        if (iconImage != null) iconImage.sprite = targetNeed.Icon;
    }

    protected void Update()
    {
        UpdateBar(targetNeed.Value / 100);
    }
    
    
}
