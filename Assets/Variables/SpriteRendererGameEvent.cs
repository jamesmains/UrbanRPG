using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Sprite Event", menuName = "Events/Sprite Event")]
public class SpriteRendererGameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<SpriteRendererGameEventListener> eventListeners = 
        new List<SpriteRendererGameEventListener>();

    public void Raise(SpriteRenderer spriteRenderer)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(spriteRenderer);
    }

    public void RegisterListener(SpriteRendererGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(SpriteRendererGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
