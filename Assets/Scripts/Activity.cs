using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;


public class Activity : MonoBehaviour
{
    [FoldoutGroup("Details")] public string activityName;
    [FoldoutGroup("Details")] [TextArea] public string activityDescription;
    [FoldoutGroup("Status")] public bool isActive;
    [FoldoutGroup("Activity")] public List<ActivityAction> ActivityActions = new();

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

    private void OnDisable()
    {
        GameEvents.OnCloseActivityWheel.Raise();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            GameEvents.OnOpenActivityWheel.Raise(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            GameEvents.OnCloseActivityWheel.Raise();
    }
}

[Serializable]
public class ActivityAction
{
    public ActivitySignature signature;
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