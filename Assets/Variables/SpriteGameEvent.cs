using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Event", menuName = "Events/Sprite Event")]
public class SpriteGameEvent : ScriptableObject
{
    private readonly List<SpriteGameEventListener> eventListeners = 
        new List<SpriteGameEventListener>();

    public void Raise(Sprite sprite)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(sprite);
    }

    public void RegisterListener(SpriteGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(SpriteGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
