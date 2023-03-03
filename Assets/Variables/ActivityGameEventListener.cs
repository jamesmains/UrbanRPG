using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivityGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public ActivityGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public ActivityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Activity activity)
    {
        Response.Invoke(activity);
    }
}
[Serializable]
public class ActivityEvent: UnityEvent<Activity>{} 
