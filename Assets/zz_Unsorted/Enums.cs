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
    Completed
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