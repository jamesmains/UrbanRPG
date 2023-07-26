using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Window
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Vector3 offset;
    [FoldoutGroup("Details")][SerializeField] private RectTransform rect;
    [FoldoutGroup("Details")][SerializeField] private Canvas scaler;

    private void OnEnable()
    {
        GameEvents.OnShowTooltip += ShowMessage;
        GameEvents.OnHideTooltip += Hide;
    }

    private void OnDisable()
    {
        GameEvents.OnShowTooltip -= ShowMessage;
        GameEvents.OnHideTooltip -= Hide;
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        Show();
    }

    private void Update()
    {
        if (!isActive) return;
        rect.anchoredPosition = (Input.mousePosition / scaler.scaleFactor)+offset;
    }
}
