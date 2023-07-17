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
    [FoldoutGroup("Debug"), SerializeField] private InventorySlot currentTargetSlot;

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
        currentTargetSlot = incomingInventorySlotData;
        if (currentTargetSlot.storedItemData.item == null) return;
        itemNameText.text = currentTargetSlot.storedItemData.item.Name;
        Show();
    }

    private void Update()
    {
        if (!isActive) return;
        if (currentTargetSlot.storedItemData.item == null) Hide();
        rect.anchoredPosition = (Input.mousePosition / scaler.scaleFactor)+offset;
    }
}
