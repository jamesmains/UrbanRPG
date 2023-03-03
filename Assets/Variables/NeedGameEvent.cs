using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Need Event", menuName = "Events/Need Event")]
public class NeedGameEvent : ScriptableObject
{
    private readonly List<NeedGameEventListener> eventListeners = 
        new List<NeedGameEventListener>();

    public void Raise(Need need)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(need);
    }

    public void RegisterListener(NeedGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(NeedGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
