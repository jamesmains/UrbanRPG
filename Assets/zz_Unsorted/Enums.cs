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
    UNDEFINED,
    NotStarted,
    Started,
    NotCompleted,
    Completed
}

public enum MonthName
{
    Flubuary = 0,
    Smarch,
    Jul,
    Autumn,
    AutumnAgain,
    Dec
}