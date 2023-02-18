using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Unity Action Event", menuName = "Events/Unity Action Event")]
public class UnityActionGameEvent : ScriptableObject
{
    private readonly List<UnityActionGameEventListener> eventListeners = 
        new List<UnityActionGameEventListener>();

    public void Raise(UnityAction value)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(value);
    }

    public void RegisterListener(UnityActionGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(UnityActionGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
