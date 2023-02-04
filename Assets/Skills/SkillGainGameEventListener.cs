
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillGainGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public SkillGainGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public SkillGainEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Sprite icon, int value)
    {
        Response.Invoke(icon,value);
    }
}
[Serializable]
public class SkillGainEvent: UnityEvent<Sprite, int>{} 