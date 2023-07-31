using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour, UIWindow
{
    [FoldoutGroup("UI Window")]public string windowName;
    [FoldoutGroup("UI Window")] public CanvasGroup canvasGroup;
    [FoldoutGroup("UI Window")] public bool isActive;
    [FoldoutGroup("UI Window")] public int priority;
    [FoldoutGroup("UI Window")] public bool excludeInLists;
    [FoldoutGroup("UI Window")] public Sprite windowIcon;
    [FoldoutGroup("UI Window/Events")] public UnityEvent onShowWindow;
    [FoldoutGroup("UI Window/Events")] public UnityEvent onHideWindow;

    private static readonly List<Window> OpenWindows = new();

    protected virtual void OnEnable()
    {
        WindowUtility.OnOpenWindow += OpenWindow;
        WindowUtility.OnCloseWindow += CloseWindow;
    }

    protected virtual void OnDisable()
    {
        WindowUtility.OnOpenWindow -= OpenWindow;
        WindowUtility.OnCloseWindow -= CloseWindow;
    }

    private void Awake()
    {
        Hide();
    }

    private void OpenWindow(string openingWindow)
    {
        if(windowName == openingWindow)
            Show();
    }
    
    private void CloseWindow(string closingWindow)
    {
        if(windowName == closingWindow)
            Hide();
    }
    
    public void Toggle()
    {
        if (isActive)
        {
            Hide();
        }
        else Show();
    }
    
    [Button("Show Window")]
    public virtual void Show()
    {
        onShowWindow.Invoke();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        isActive = true;
        OpenWindows.Add(this);
    }

    [Button("Hide Window")]
    public virtual void Hide()
    {
        onHideWindow.Invoke();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        isActive = false;
        OpenWindows.Remove(this);
    }

    protected virtual void CloseOtherWindows(Window ActiveWindow)
    {
        var windowsToHide = new List<Window>();
        foreach (var window in OpenWindows)
        {
            if (window == ActiveWindow) continue;
            if(window.priority <= ActiveWindow.priority)
                windowsToHide.Add(window);
        }

        foreach (var window in windowsToHide)
        {
            window.Hide();
        }
    }
}
