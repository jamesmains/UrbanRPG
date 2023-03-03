using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public DialogueGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public DialogueEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Dialogue dialogue)
    {
        Response.Invoke(dialogue);
    }
}
[Serializable]
public class DialogueEvent: UnityEvent<Dialogue>{}