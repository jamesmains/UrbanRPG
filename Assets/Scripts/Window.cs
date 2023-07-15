using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Window : MonoBehaviour, UIWindow
{
#if UNITY_EDITOR
    [FoldoutGroup("UI Window")]public string description;
#endif
    [FoldoutGroup("UI Window")] public CanvasGroup canvasGroup;
    [FoldoutGroup("UI Window")] public bool isActive;
    [FoldoutGroup("UI Window")] public Sprite windowIcon;

    private void Awake()
    {
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
        StopAllCoroutines();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        isActive = true;
    }

    [Button("Hide Window")]
    public virtual void Hide()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        isActive = false;
    }
}
