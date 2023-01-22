using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI descriptionText;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected RectTransform fillBarImageRect;
    [SerializeField] protected float fillSpeed;
    [SerializeField] protected Vector2 fillBarSizeRange;
    
    protected virtual void UpdateBar(float value)
    {
        Vector2 size = fillBarImageRect.sizeDelta;
        size.x = (fillBarSizeRange.y - fillBarSizeRange.x) * (value) + fillBarSizeRange.x;
        fillBarImageRect.sizeDelta = Vector2.Lerp(fillBarImageRect.sizeDelta,size, fillSpeed * Time.deltaTime);
    }
}
