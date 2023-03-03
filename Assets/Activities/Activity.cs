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
    [FoldoutGroup("Data")] [SerializeField] private BoolVariable mouseOverUserInterface;
    [FoldoutGroup("Data")] public float rangeRequirement = 2;
    [FoldoutGroup("Data")] public bool isActive;
    public List<ActivityAction> ActivityActions;

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
    
    private void OnMouseOver()
    {
        if (!mouseOverUserInterface.Value && !isActive)
        {
            isActive = true;
        }
        else if (mouseOverUserInterface.Value && isActive)
        {
            Clear();
        }
    }

    private void Clear()
    {
        isActive = false;
    }
}

[Serializable]
public class ActivityAction
{
    public ActivitySignature signature;
    [FoldoutGroup("Event")] public UnityEvent eventChannel;

    public void AssignListeners(Action[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            var actionIndex = i;
            eventChannel.AddListener(delegate {actions[actionIndex].Invoke();});
        }
    }

    public void AssignListener(Action action)
    {
        eventChannel.AddListener(action.Invoke);        
    }
}