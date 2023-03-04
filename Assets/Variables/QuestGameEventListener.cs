using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public QuestGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public QuestEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Quest quest)
    {
        Response.Invoke(quest);
    }
}
[Serializable]
public class QuestEvent: UnityEvent<Quest>{}