using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Quest Task Event", menuName = "Events/Quest Task Event")]
public class QuestTaskGameEvent : ScriptableObject
{
    private readonly List<QuestTaskGameEventListener> eventListeners = 
        new List<QuestTaskGameEventListener>();

    public void Raise(QuestTaskSignature questTaskSignature)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(questTaskSignature);
    }

    public void RegisterListener(QuestTaskGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(QuestTaskGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
