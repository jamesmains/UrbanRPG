using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using UnityEngine;
using UnityEngine.Events;

public class InventorySlotGameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public InventorySlotGameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public InventoryItemManagementEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(InventorySlot item)
    {
        Response.Invoke(item);
    }
}
[Serializable]
public class InventoryItemManagementEvent: UnityEvent<InventorySlot>{} 