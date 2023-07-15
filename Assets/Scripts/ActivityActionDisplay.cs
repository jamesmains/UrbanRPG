using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActivityActionDisplay : MonoBehaviour
{
    [FoldoutGroup("Display")][SerializeField] private Image actionIconDisplay;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI actionNameText;
    [FoldoutGroup("Data")][SerializeField] private Color highlightColor;
    [FoldoutGroup("Data")][SerializeField] private Color normalColor;
    
    public static ActivityActionDisplay Current;

    public void Setup(ActivityAction incomingAction, string extraText = "", bool useIcon = true)
    {
        actionNameText.text = $"{incomingAction.signature.ActionName}{extraText}";
        
        if (useIcon)
            actionIconDisplay.sprite = incomingAction.signature.ActionIcon;
        else actionIconDisplay.enabled = false;
    }

    public void SetHighlightState(bool state)
    {
        actionNameText.enabled = state;
    }
}
