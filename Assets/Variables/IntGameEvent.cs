using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Int Event", menuName = "Events/Int Event")]
public class IntGameEvent : ScriptableObject
{
    private readonly List<IntGameEventListener> eventListeners = 
        new List<IntGameEventListener>();

    public void Raise(int value)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(value);
    }

    public void RegisterListener(IntGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(IntGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
