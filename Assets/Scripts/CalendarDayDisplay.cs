using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CalendarDayDisplay : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Image frameIcon;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image dayIcon;

    private UnityAction onInteract;
    [HideInInspector]public Window parentWindow;
    private bool isMouseOver;
    private bool isHighlighted;
    
    int clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;

    public static CalendarDayDisplay LastInteractedDayDisplay;

    private void OnEnable()
    {
        GameEvents.OnPrimaryMouseButtonUp += Interact;
    }

    private void OnDisable()
    {
        GameEvents.OnPrimaryMouseButtonUp -= Interact;
    }
    
    public void Setup(bool highlight, int day, Sprite specialDaySprite, Window parent)
    {
        isHighlighted = highlight;
        frameIcon.color = highlight ? Color.white : Color.gray;
        dayText.text = (day+1).ToString();
        parentWindow = parent;
        if (specialDaySprite != null)
        {
            dayIcon.sprite = specialDaySprite;
        }
        else
        {
            dayIcon.enabled = false;
        }
    }

    public void ToggleTemporaryHighlight(bool state)
    {
        if (!parentWindow.isActive) return;
        if (LastInteractedDayDisplay != null && LastInteractedDayDisplay != this)
        {
            LastInteractedDayDisplay.ToggleTemporaryHighlight(false);
        }

        LastInteractedDayDisplay = this;
        frameIcon.color = state ? Color.cyan : isHighlighted ? Color.white : Color.gray;
    }

    public void Interact()
    {
        if (!parentWindow.isActive) return;
        if (isMouseOver)
        {
            if (IsDoubleClick())
            {
                print("Double click");
                GameEvents.OpenCalendarDayDetails.Raise();
            }
            else onInteract.Invoke();
        }
    }

    private bool IsDoubleClick()
    {
        clicked++;
        if (clicked == 1) clicktime = Time.time;
 
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;

        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;

        return false;
    }

    public void AssignInteract(UnityAction interactActions)
    {
        onInteract = interactActions;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}
