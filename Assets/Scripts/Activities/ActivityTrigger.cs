using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;


public class ActivityTrigger : SerializedMonoBehaviour
{
    [SerializeField, FoldoutGroup("Details"),TextArea] private string description;
    [ReadOnly] public bool isActive;
    [OdinSerialize] public List<ActivityAction> Activities = new();

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
            isActive = false;
            ActivityLock = false;
        };
        GameEvents.OnEndActivity += delegate
        {
            isActive = false;
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
        if (isActive || ActivityLock || Global.PlayerLock > 0) return;
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

public class ActivityAction
{
    public Activity signature;
    [ShowInInspector,OdinSerialize] public List<Condition> specialConditions { get; set; } = new();
    [FoldoutGroup("World Action")] public UnityEvent worldActions = new();

    public void InvokeSpecialActions()
    {
        specialConditions.ForEach(c => c.Use());
    }
    
    public bool IsConditionMet()
    {
        if (specialConditions.Count == 0) return true;
        return specialConditions.TrueForAll(c => c.IsConditionMet());
    }
    
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