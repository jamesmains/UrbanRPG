
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteRendererGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public SpriteRendererGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public SpriteRendererEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(SpriteRenderer spriteRenderer)
    {
        Response.Invoke(spriteRenderer);
    }
}
[Serializable]
public class SpriteRendererEvent: UnityEvent<SpriteRenderer>{} 