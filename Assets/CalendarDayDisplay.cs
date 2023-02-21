using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarDayDisplay : MonoBehaviour
{
    [SerializeField] private Image frameIcon;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private Image dayIcon;

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
}
