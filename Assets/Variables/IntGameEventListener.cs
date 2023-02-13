using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public IntGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public IntEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int value)
    {
        Response.Invoke(value);
    }
}
[Serializable]
public class IntEvent: UnityEvent<int>{} 
