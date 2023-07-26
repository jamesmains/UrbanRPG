using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    [FoldoutGroup("Display")][SerializeField] protected TextMeshProUGUI nameText;
    [FoldoutGroup("Display")][SerializeField] protected TextMeshProUGUI descriptionText;
    [FoldoutGroup("Display")][SerializeField] protected Image iconImage;
    [FoldoutGroup("Display")][SerializeField] protected RectTransform fillBarImageRect;
    [FoldoutGroup("Display")][SerializeField] protected Image fillBarImage;
    [FoldoutGroup("Data")][SerializeField] protected float fillSpeed;
    [FoldoutGroup("Data")][SerializeField] protected Vector2 fillBarSizeRange;

    protected virtual void UpdateBar(float value)
    {
        Vector2 size = fillBarImageRect.sizeDelta;
        size.x = (fillBarSizeRange.y - fillBarSizeRange.x) * (value) + fillBarSizeRange.x;
        fillBarImageRect.sizeDelta = Vector2.Lerp(fillBarImageRect.sizeDelta,size, fillSpeed * Time.deltaTime);
    }

    protected virtual void UpdateRadialBar(float value)
    {
        fillBarImage.fillAmount = Mathf.Lerp(fillBarImage.fillAmount, value, Time.deltaTime * fillSpeed);
    }
}
