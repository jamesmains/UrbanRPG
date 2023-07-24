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
    public ActivityAction activityAction;
    [FoldoutGroup("Display")][SerializeField] private Image actionIconDisplay;
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI actionNameText;
    [FoldoutGroup("Data")][SerializeField] private Color highlightColor;
    [FoldoutGroup("Data")][SerializeField] private Color normalColor;
    
    public static ActivityActionDisplay Current;

    public void Setup(ActivityAction incomingAction, string extraText = "", bool useIcon = true)
    {
        activityAction = incomingAction;
        actionNameText.text = $"{activityAction.signature.ActivityName}{extraText}";
        
        if (useIcon)
            actionIconDisplay.sprite = activityAction.signature.ActivityIcon;
        else actionIconDisplay.enabled = false;
    }

    public void SetHighlightState(bool state)
    {
        actionNameText.enabled = state;
    }
}
