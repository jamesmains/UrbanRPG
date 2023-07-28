using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseInteractionEffects : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [ShowInInspector,OdinSerialize] public List<HoverEffect> Effects { get; set; } = new();

    private void Awake()
    {
        foreach (var effect in Effects)
        {
            effect.SetupEffect();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var effect in Effects)
        {
            effect.OnMouseEnter();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var effect in Effects)
        {
            effect.OnMouseExit();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var effect in Effects)
        {
            effect.OnMouseDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var effect in Effects)
        {
            effect.OnMouseUp();
        }
    }
}

[Serializable]
public abstract class HoverEffect
{
    public abstract void SetupEffect();
    public abstract void OnMouseUp();
    public abstract void OnMouseDown();
    public abstract void OnMouseEnter();
    public abstract void OnMouseExit();
}

public class ShadowPopEffect : HoverEffect
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Vector2 imagePopDistance;
    [SerializeField] private Vector2 shadowPopDistance;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly]private RectTransform cachedRect;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly]private Vector2 cachedStartPosition;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly]private Shadow shadow;
    [SerializeField,FoldoutGroup("Debug"),ReadOnly]private bool isMouseOver;

    public override void SetupEffect()
    {
        var targetGO = targetImage.gameObject;
        if (targetGO.GetComponent<Shadow>() == null)
        {
            targetGO.AddComponent<Shadow>();
        }
        cachedRect = targetGO.GetComponent<RectTransform>();
        cachedStartPosition = cachedRect.anchoredPosition;
        shadow = targetGO.GetComponent<Shadow>();
        shadow.effectDistance = Vector2.zero;
    }

    public override void OnMouseUp()
    {
        if (!isMouseOver) return;
        cachedRect.anchoredPosition = cachedStartPosition + imagePopDistance;
        shadow.effectDistance = shadowPopDistance;
    }

    public override void OnMouseDown()
    {
        if (!isMouseOver) return;
        cachedRect.anchoredPosition = cachedStartPosition;
        shadow.effectDistance = Vector2.zero;
    }

    public override void OnMouseEnter()
    {
        isMouseOver = true;
        cachedRect.anchoredPosition = cachedStartPosition + imagePopDistance;
        shadow.effectDistance = shadowPopDistance;
    }

    public override void OnMouseExit()
    {
        isMouseOver = false;
        cachedRect.anchoredPosition = cachedStartPosition;
        shadow.effectDistance = Vector2.zero;
    }
}

public class ChangeTextEffect : HoverEffect
{
    public ChangeTextEffect(TextMeshProUGUI display, string incomingMessage)
    {
        if (display == null)
        {
            Debug.LogError("No Display for ChangeTextEffect");
            return;
        }

        display.text = "";
        targetTextObject = display;
        message = incomingMessage;
    }
    
    [SerializeField] private TextMeshProUGUI targetTextObject;
    [SerializeField] private string targetTextObjectTag;
    [SerializeField] private string message;
    
    public override void SetupEffect()
    {
        if(targetTextObject == null && !string.IsNullOrEmpty(targetTextObjectTag))
            targetTextObject = GameObject.FindWithTag(targetTextObjectTag).GetComponent<TextMeshProUGUI>();
    }

    public override void OnMouseUp()
    {
        
    }

    public override void OnMouseDown()
    {
        
    }

    public override void OnMouseEnter()
    {
        targetTextObject.text = message;
    }

    public override void OnMouseExit()
    {
        targetTextObject.text = "";
    }
}