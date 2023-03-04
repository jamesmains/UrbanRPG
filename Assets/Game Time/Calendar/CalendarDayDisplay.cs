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

    private UnityAction onMouseOver;
    private UnityAction onMouseExit;
    private UnityAction onInteract;

    private bool isMouseOver;
    private bool isHighlighted;

    public static CalendarDayDisplay LastInteractedDayDisplay;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseOver) // Todo need support for controllers and other inputs
        {
            Interact();
        }
    }
    
// TODO need better highlighting for current day and selected day display

    public void Setup(bool highlight, int day, Sprite specialDaySprite)
    {
        isHighlighted = highlight;
        frameIcon.color = highlight ? Color.white : Color.gray;
        dayText.text = (day+1).ToString();
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
        if (LastInteractedDayDisplay != null && LastInteractedDayDisplay != this)
        {
            LastInteractedDayDisplay.ToggleTemporaryHighlight(false);
        }

        LastInteractedDayDisplay = this;
        frameIcon.color = state ? Color.cyan : isHighlighted ? Color.white : Color.gray;
    }

    public void Interact()
    {
        onInteract.Invoke();
    }

    public void AssignMouseOver(UnityAction mouseOver)
    {
        onMouseOver = mouseOver;
    }
    
    public void AssignMouseExit(UnityAction mouseExit)
    {
        onMouseExit = mouseExit;
    }

    public void AssignInteract(UnityAction interactActions)
    {
        onInteract = interactActions;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseOver?.Invoke();
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseExit?.Invoke();
        isMouseOver = false;
    }
}
