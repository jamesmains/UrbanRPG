using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemNameDisplay : Window
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Vector3 offset;
    [FoldoutGroup("Details")][SerializeField] private RectTransform rect;
    [FoldoutGroup("Details")][SerializeField] private Canvas scaler;

    private void OnEnable()
    {
        GameEvents.OnMouseEnterInventorySlot += ShowItemNameDisplay;
        GameEvents.OnMouseExitInventorySlot += Hide;
    }

    private void OnDisable()
    {
        GameEvents.OnMouseEnterInventorySlot -= ShowItemNameDisplay;
        GameEvents.OnMouseExitInventorySlot -= Hide;
    }

    private void ShowItemNameDisplay(InventorySlot incomingInventorySlotData)
    {
        if (incomingInventorySlotData.storedItem.item == null) return;
        itemNameText.text = incomingInventorySlotData.storedItem.item.Name;
        Show();
    }

    private void Update()
    {
        if(isActive)
            rect.anchoredPosition = (Input.mousePosition / scaler.scaleFactor)+offset;
    }
}
