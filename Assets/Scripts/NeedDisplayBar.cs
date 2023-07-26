using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedDisplayBar : DisplayBar
{
    public Need targetNeed;
    
    protected void Update()
    {
        UpdateBar(targetNeed.Value / 100);
    }
    
    public void Setup()
    {
        if(nameText!=null) nameText.text = targetNeed.Name;
        if(descriptionText!=null) descriptionText.text = targetNeed.Description;
        if (iconImage != null) iconImage.sprite = targetNeed.Icon;
    }
}
