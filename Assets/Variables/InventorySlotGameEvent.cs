using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;
[CreateAssetMenu(fileName = "Inventory Item Event", menuName = "Events/Inventory Item Event")]
public class InventorySlotGameEvent : ScriptableObject
{
    private readonly List<InventorySlotGameEventListener> eventListeners = 
        new List<InventorySlotGameEventListener>();

    public void Raise(InventorySlot item)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(item);
    }

    public void RegisterListener(InventorySlotGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(InventorySlotGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
