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
    UNDEFINED,
    Quest,
    Ride
}

public enum GearType
{
    GearBody,
    GearShoes,
    GearPants,
    GearMouth,
    GearEyes,
    GearShirt,
    GearHair,
    GearAccessory
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

public enum InputType
{
    Keyboard,
    Gamepad,
    MouseWheel
}

public enum InputActionName
{
    MoveLeft, MoveRight, MoveUp, MoveDown, Interact, ToggleRide, Scroll, PrimaryMouseButton, AlternateMouseButton
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
        foreach (Transform child in t)
        {
            Object.DestroyImmediate(child.gameObject);
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

public static class UrbanDebugger
{
    public static int DebugLevel = 0; // 0 None, 1 Low, 2 Med, 3 High
}

public static class Global
{
    public static bool IsMouseOverUI;
    public static int PlayerLock;
}