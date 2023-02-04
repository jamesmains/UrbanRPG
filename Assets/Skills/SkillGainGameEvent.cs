using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Skill Gain Event", menuName = "Events/Skill Gain Event")]
public class SkillGainGameEvent : ScriptableObject
{
    private readonly List<SkillGainGameEventListener> eventListeners = 
        new List<SkillGainGameEventListener>();

    public void Raise(Sprite icon, int value)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(icon,value);
    }

    public void RegisterListener(SkillGainGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(SkillGainGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
