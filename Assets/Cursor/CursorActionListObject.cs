using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorActionListObject : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image actionIconDisplay;
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color normalColor;
    
    public static CursorActionListObject Current;

    
    private const float highlightSize = 42;
    private const float normalSize = 32;

    public void Setup(ActivityAction incomingAction, string extraText = "", bool useIcon = true)
    {
        actionNameText.text = $"{incomingAction.signature.ActionName}{extraText}";
        
        if (useIcon)
            actionIconDisplay.sprite = incomingAction.signature.ActionIcon;
        else actionIconDisplay.enabled = false;
        
        ToggleHighlight(false);
    }

    public void ToggleHighlight(bool state)
    {
        if (state)
        {
            if (Current != null)
            {
                if (Current != this)
                {
                    Current.ToggleHighlight(false);
                }
            }

            if (Current == this) return;
            StopAllCoroutines();
            StartCoroutine(SetDisplaySizes(highlightSize));
            Current = this;
            actionNameText.color = highlightColor;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SetDisplaySizes(normalSize));
            actionNameText.color = normalColor;
        }
    }

    IEnumerator SetDisplaySizes(float size)
    {
        while (Math.Abs(actionNameText.fontSize - size)  > 0.0001 || Math.Abs(rectTransform.sizeDelta.y - size) > 0.0001)
        {
            Vector2 rectSize = rectTransform.sizeDelta;
            actionNameText.fontSize = Mathf.Lerp(actionNameText.fontSize, size, Time.deltaTime * transitionSpeed); 
            rectSize.y = Mathf.Lerp(rectSize.y,size,Time.deltaTime * transitionSpeed);
            rectTransform.sizeDelta = rectSize;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
