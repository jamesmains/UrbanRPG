using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// Presume target is always going to be player since this relies on keyboard or gamepad input
public class InteractionTrigger : MonoBehaviour
{
    // Todo needs some reworking
    [SerializeField] private bool requireKeyPress;
    [SerializeField] private InteractionType interactionType;
    [SerializeField, ShowIf("requireKeyPress",true)] private KeyCode interactionKey; // todo replace with scriptable objects for key rebinding
    [FoldoutGroup("Data"), ShowIf("requireKeyPress",true)][SerializeField] private IntVariable playerLock;
    [FoldoutGroup("Data")]public bool readyToInteract;
    [SerializeField] private UnityEvent onInteract;

    public static InteractionTrigger targetTrigger; // try to prevent multiple interactions at once

    private void Update()
    {
        if (Input.GetKeyDown(interactionKey) && targetTrigger == this && playerLock.Value == 0)
        {
            onInteract.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactionType == InteractionType.OnEnter && !requireKeyPress)
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
