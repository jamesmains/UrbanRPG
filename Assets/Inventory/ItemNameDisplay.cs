using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNameDisplay : MonoBehaviour
{
    [SerializeField] private StringVariable itemNameVariable;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [FoldoutGroup("Details")][SerializeField] private CanvasGroup canvasGroup;
    [FoldoutGroup("Details")][SerializeField] private RectTransform rect;
    [FoldoutGroup("Details")][SerializeField] private Canvas scaler;

    private void Awake()
    {
        HideItemName();
    }

    private void Update()
    {
        rect.anchoredPosition = Input.mousePosition / scaler.scaleFactor;
        itemNameText.text = itemNameVariable.Value;
    }

    public void DisplayItemName()
    {
        canvasGroup.alpha = 1;
    }

    public void HideItemName()
    {
        canvasGroup.alpha = 0;
    }
}
