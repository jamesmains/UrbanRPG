using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNameDisplay : MonoBehaviour
{
    [SerializeField] private StringVariable itemNameVariable;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Canvas scaler;

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
