using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


public class ActivityTrigger : MonoBehaviour
{
    [ReadOnly] public bool isActive;
    public List<ActivityAction> Activities = new();

    public static bool ActivityLock;

    private string identity;
    public string Identity
    {
        get
        {
            if (string.IsNullOrEmpty(identity))
            {
                identity = Guid.NewGuid().ToString();
            }
            return identity;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnStartActivity += delegate
        {
            ActivityLock = true;
            isActive = false;
            GameEvents.OnCloseActivityWheel.Raise();
        };
        GameEvents.OnCancelActivity += delegate
        {
            ActivityLock = false;
        };
        GameEvents.OnEndActivity += delegate
        {
            ActivityLock = false;
        };

        GameEvents.OnCloseActivityWheel += delegate
        {
            isActive = false;
        };
    }

    private void OnDisable()
    {
        GameEvents.OnCloseActivityWheel.Raise();
        
        GameEvents.OnStartActivity -= delegate
        {
            ActivityLock = true;
            isActive = false;
            GameEvents.OnCloseActivityWheel.Raise();
        };
        GameEvents.OnCancelActivity -= delegate
        {
            ActivityLock = false;
        };
        GameEvents.OnEndActivity -= delegate
        {
            ActivityLock = false;
        };
        
        GameEvents.OnCloseActivityWheel -= delegate
        {
            isActive = false;
        };

        isActive = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive || ActivityLock) return;
        if (other.CompareTag("Player"))
        {
            GameEvents.OnOpenActivityWheel.Raise(this);
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameEvents.OnCloseActivityWheel.Raise();
            isActive = false;
        }
    }
}

[Serializable]
public class ActivityAction
{
    public Activity signature;
    [FoldoutGroup("World Action")] public UnityEvent worldActions = new();

    public void AssignListeners(Action[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            var actionIndex = i;
            worldActions.AddListener(delegate {actions[actionIndex].Invoke();});
        }
    }

    public void AssignListener(Action action)
    {
        worldActions.AddListener(action.Invoke);        
    }
}