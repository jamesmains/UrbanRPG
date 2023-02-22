using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Calendar Signature", menuName = "Signatures/Calendar Signature")]
public class CalendarSignature : SerializedScriptableObject
{
    public bool Active;
    public string DisplayName;
    public string DisplayText;
    public Sprite DisplayIcon;

    [field: SerializeField]
    public List<CalendarCondition> Conditions { get; private set; } = new List<CalendarCondition>();
    
    public bool IsConditionMet(int day, int month)
    {
        return Conditions.TrueForAll(c => c.IsConditionMet(day,month));
    }
}

[Serializable]
public abstract class CalendarCondition
{
    public abstract bool IsConditionMet(int day, int month);
}

public class CalendarAndCondition : CalendarCondition
{
    [field: SerializeField]
    public List<CalendarCondition> Conditions { get; private set; } = new List<CalendarCondition>();
    
    public override bool IsConditionMet(int day, int month)
    {
        return Conditions.TrueForAll(c => c.IsConditionMet(day,month));
    }
}

public class CalendarOrCondition : CalendarCondition
{
    [field: SerializeField]
    public List<CalendarCondition> Conditions { get; private set; } = new List<CalendarCondition>();

    public override bool IsConditionMet(int day, int month)
    {
        return Conditions.Exists(c => c.IsConditionMet(day,month));
    }
}

public class CalendarNotCondition : CalendarCondition
{
    [field: SerializeField]
    public List<CalendarCondition> Conditions { get; private set; } = new List<CalendarCondition>();

    public override bool IsConditionMet(int day, int month)
    {
        return !Conditions.Exists(c => c.IsConditionMet(day,month));
    }
}

public class DayOfWeekCondition : CalendarCondition
{
    [field: SerializeField]
    public Day TargetDay { get; private set; }
    public override bool IsConditionMet(int day, int month)
    {
        day %= 7;
        return (day == (int) TargetDay);
    }
}

public class SpecificDayCondition : CalendarCondition
{
    [field: SerializeField, Range(0,27)]
    public int TargetDate { get; private set; }
    public override bool IsConditionMet(int day, int month)
    {
        return day == TargetDate;
    }
}

public class SpecificMonthCondition : CalendarCondition
{
    [field: SerializeField]
    public Month TargetMonth { get; private set; }
    public override bool IsConditionMet(int day, int month)
    {
        return month == (int)TargetMonth;
    }
}

public class DateRangeCondition : CalendarCondition
{
    [field: SerializeField, MinMaxSlider(0, 27)]
    public Vector2Int Range;
    public override bool IsConditionMet(int day, int month)
    {
        return day >= Range.x && day <= Range.y;
    }
}

