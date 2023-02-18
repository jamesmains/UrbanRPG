using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityActionGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public UnityActionGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityActionEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(UnityAction value)
    {
        Response.Invoke(value);
    }
}
[Serializable]
public class UnityActionEvent: UnityEvent<UnityAction>{}