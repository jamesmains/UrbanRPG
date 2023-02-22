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

    public void Setup(bool highlight, int day, Sprite specialDaySprite)
    {
        frameIcon.color = highlight ? Color.white : Color.gray;
        dayText.text = day.ToString();
        if (specialDaySprite != null)
        {
            dayIcon.sprite = specialDaySprite;
        }
        else
        {
            dayIcon.enabled = false;
        }
    }

    public void AssignMouseOver(UnityAction mouseOver)
    {
        onMouseOver = mouseOver;
    }
    
    public void AssignMouseExit(UnityAction mouseExit)
    {
        onMouseExit = mouseExit;
    }

    private void OnMouseEnter()
    {
        onMouseOver.Invoke();
    }

    private void OnMouseExit()
    {
        onMouseExit.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Enter pointer");
        onMouseOver?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Exit Pointer");
        onMouseExit?.Invoke();
    }
}
