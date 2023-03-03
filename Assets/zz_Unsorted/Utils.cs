using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionType
{
    UNDEFINED,
    Inspect,
    Harvest,
    Interact,
}

public enum CursorState
{
    Default,
    DisplayList
}

public enum ItemType
{
    UNDEFINED,
    Tool
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
    Flubuary = 0,
    Smarch,
    Jul,
    Autumn,
    AutumnAgain,
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

