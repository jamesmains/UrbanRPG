using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;
[CreateAssetMenu(fileName = "Inventory Item Event", menuName = "Events/Inventory Item Event")]
public class InventoryItemManagementGameEvent : ScriptableObject
{
    private readonly List<InventoryItemManagementGameEventListener> eventListeners = 
        new List<InventoryItemManagementGameEventListener>();

    public void Raise(InventorySlot item)
    {
        for(int i = eventListeners.Count -1; i >= 0; i--)
            eventListeners[i].OnEventRaised(item);
    }

    public void RegisterListener(InventoryItemManagementGameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(InventoryItemManagementGameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
