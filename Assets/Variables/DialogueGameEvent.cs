using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Dialogue Event", menuName = "Events/Dialogue Event")]
public class DialogueGameEvent : ScriptableObject
{
    private readonly List<DialogueGameEventListener> eventListeners = 
        new List<DialogueGameEventListener>();

    public void Raise(Dialogue dialogue)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(dialogue);
    }

    public void RegisterListener(DialogueGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(DialogueGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
