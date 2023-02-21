using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Calendar Signature", menuName = "Signatures/Calendar Signature")]
public class CalendarSignature : ScriptableObject, ICalendarSignature
{
    public bool Active;
    public string DisplayName;
    public string DisplayText;
    public Sprite DisplayIcon;

    public int Day =-1;
    public int Month =-1;
    public int Year =-1;

    public virtual bool IsToday(int day, int month, int year)
    {
        return (Day == day || Day == -1) && (Month == month || Month == -1) && (Year == year || Year == -1);
    }
}

