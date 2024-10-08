using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public enum ActivityType
{
    UNDEFINED,
    Inspect,
    Consume,
    Interact,
}

public enum ItemType
{
    Junk,
    Item,
    Tool,
    Quest,
    Ride
}

public enum EffectType
{
    Buff,
    Debuff
}

public enum GearType
{
    Body,
    Shoes,
    Pants,
    Mouth,
    Eyes,
    Shirt,
    Hair,
    Accessory,
    
    Ride
}

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
}

public enum QuestState
{
    NotStarted,
    Started,
    Completed,
    ReadyToComplete
}

public enum QuestType
{
    Standard,
    Dream,
    Repeatable
}

public enum Month
{
    Jan = 0,
    Feb,
    Mar,
    Apr,
    May,
    Jun,
    Jul,
    Aug,
    Sep,
    Oct,
    Nov,
    Dec
}

public enum Day
{
    Saturday = 0,
    Sunday = 1,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday
}

public enum InteractionType
{
    OnEnter,
    OnStay,
    OnExit
}

// yoinked from https://stackoverflow.com/questions/20156/is-there-an-easy-way-to-create-ordinals-in-c
public static class UtilFunctions
{
    public static string AddOrdinal(int num)
    {
        if( num <= 0 ) return num.ToString();

        switch(num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }
    
        switch(num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }
}

public static class ExtensionMethods
{
    public static void SetChildrenActiveState(this Transform t, bool state)
    {
        foreach (Transform child in t)
        {
            child.gameObject.SetActive(state);
        }
    }
    
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildrenInEditor(this Transform t)
    {
        while (t.childCount > 0)
        {
            Object.DestroyImmediate(t.GetChild(t.childCount-1).gameObject);
        }
    }
    
    public static void DestroyChildren(this Transform t, List<GameObject> list)
    {
        foreach (Transform child in t)
        {
            GameObject.Destroy(child.gameObject);
        }
        list.Clear();
    }
}


public static class Global
{
    public static bool IsMouseOverUI;
    public static int PlayerLock;
}