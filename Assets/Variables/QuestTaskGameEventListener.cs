using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestTaskGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public QuestTaskGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public QuestTaskEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(QuestTaskSignature questTaskSignature)
    {
        Response.Invoke(questTaskSignature);
    }
}
[Serializable]
public class QuestTaskEvent: UnityEvent<QuestTaskSignature>{}
