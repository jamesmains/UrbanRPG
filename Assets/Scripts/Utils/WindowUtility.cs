using System.Collections;
using System.Collections.Generic;
using i302.Utils.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "Window Utility", menuName = "Window Utility")]
public class WindowUtility : ScriptableObject
{
    
    public static i302Event<string> OnOpenWindow = new();
    public static i302Event<string> OnCloseWindow = new();
    
    public void OpenWindow(string windowName)
    {
        OnOpenWindow.Raise(windowName);
    }

    public void CloseWindow(string windowName)
    {
        OnCloseWindow.Raise(windowName);
    }
}
