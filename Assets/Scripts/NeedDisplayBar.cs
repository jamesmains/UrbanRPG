using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NeedDisplayBar : DisplayBar, IPointerEnterHandler, IPointerExitHandler
{
    public Need targetNeed;
    
    protected void Update()
    {
        UpdateRadialBar(targetNeed.Value / 100);
    }
    
    public void Setup()
    {
        if(nameText!=null) nameText.text = targetNeed.Name;
        if(descriptionText!=null) descriptionText.text = targetNeed.Description;
        if (iconImage != null) iconImage.sprite = targetNeed.Icon;
        if (fillBarImage != null) fillBarImage.sprite = targetNeed.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEvents.OnShowTooltip.Raise($"{targetNeed.Name}\n{targetNeed.Description}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.OnHideTooltip.Raise();
    }
}
