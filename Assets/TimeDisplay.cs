using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TimeVariable currentTimeVariable;
    [SerializeField] private TextMeshProUGUI timeDisplayText;
    public bool use12HourClock;

    private void Update()
    {
        float hour = (int) Math.Truncate(currentTimeVariable.Value / 60f);
        float minute = currentTimeVariable.Value % 60f;
        string addendum = "";
        if (use12HourClock)
        {
            addendum = hour > 11 ? "PM" : "AM";
            if (hour > 12)
            {
                hour -= 12;    
            }
        }
        timeDisplayText.text = $"{hour:00}:{minute:00} {addendum}";
    }
}
