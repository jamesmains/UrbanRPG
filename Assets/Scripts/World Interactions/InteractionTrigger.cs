using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// Presume target is always going to be player since this relies on keyboard or gamepad input
public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private bool requireKeyPress;
    [SerializeField] private InteractionType interactionType;
    [FoldoutGroup("Data")]public bool readyToInteract;
    [SerializeField] private UnityEvent onInteract;

    public static InteractionTrigger targetTrigger; // try to prevent multiple interactions at once

    private void OnEnable()
    {
        GameEvents.OnInteractButtonDown += Trigger;
    }

    private void OnDisable()
    {
        GameEvents.OnInteractButtonDown -= Trigger;
    }

    private void Trigger()
    {
        if (targetTrigger != this && Global.PlayerLock > 0) return;
        onInteract.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactionType == InteractionType.OnEnter && !requireKeyPress)
        {
            Trigger();
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
