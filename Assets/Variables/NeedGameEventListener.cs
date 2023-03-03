using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeedGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public NeedGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public NeedEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Need need)
    {
        Response.Invoke(need);
    }
}
[Serializable]
public class NeedEvent: UnityEvent<Need>{} 