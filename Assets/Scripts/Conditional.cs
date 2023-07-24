using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;


#region Generic

[Serializable]
public abstract class Condition
{
    public abstract bool IsConditionMet();
    public abstract void Use();
}

public class AndCondition : Condition
{
    [field: SerializeField]
    public List<Condition> Conditions { get; private set; } = new();
    
    public override bool IsConditionMet()
    {
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }
    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class OrCondition : Condition
{
    [field: SerializeField]
    public List<Condition> Conditions { get; private set; } = new();

    public override bool IsConditionMet()
    {
        return Conditions.Exists(c => c.IsConditionMet());
    }
    
    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

public class NotCondition : Condition
{
    [field: SerializeField]
    public List<Condition> Conditions { get; private set; } = new();

    public override bool IsConditionMet()
    {
        return !Conditions.Exists(c => c.IsConditionMet());
    }
    
    public override void Use()
    {
        Conditions.ForEach(c => c.Use());
    }
}

#endregion

#region Calendar

public abstract class CalendarCondition : Condition
{
    public abstract bool IsConditionMet(int day, int month);
    public override bool IsConditionMet()
    {
        throw new NotImplementedException();
    }

    public override void Use()
    {
        throw new NotImplementedException();
    }
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

#endregion

public class QuestStateConditional : Condition
{
    [field: SerializeField] private Quest Quest { get; set; }
    [field: SerializeField] private QuestState State { get; set; }
    [field: SerializeField] private QuestTask Step { get; set; }
    public override bool IsConditionMet()
    {
        return (Quest.CurrentState == State && Quest.CurrentStep == Step);
    }

    public override void Use()
    {
        
    }
}

public class ItemConditional : Condition
{
    [field: SerializeField]
    public Item Item { get; private set; }
    [field: SerializeField]
    public int RequiredAmount { get; private set; }
    [field: SerializeField]
    public bool UseAny { get; private set; }
    [field: SerializeField]
    public bool ConsumeOnUse { get; private set; }
    [field: SerializeField]
    public Inventory Inventory { get; private set; }
    
    public override bool IsConditionMet()
    {
        return Inventory.HasItem(Item, RequiredAmount);
    }

    public override void Use()
    {
        if(ConsumeOnUse) Inventory.TryUseItem(Item,RequiredAmount);
    }
}






