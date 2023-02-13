using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Presume target is always going to be player since this relies on keyboard or gamepad input
public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey; // todo replace with scriptable objects for key rebinding
    public bool readyToInteract;
    [SerializeField] private IntVariable playerLock;
    [SerializeField] private UnityEvent onInteract;

    public static InteractionTrigger targetTrigger; // try to prevent multiple interactions at once

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey) && targetTrigger == this && playerLock.Value == 0)
        {
            onInteract.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !readyToInteract)
        {
            readyToInteract = true;
            targetTrigger = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && readyToInteract)
        {
            readyToInteract = false;
            if (targetTrigger == this)
            {
                targetTrigger = null;
            }
        }
    }
}
