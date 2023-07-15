using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    // Need to make this more robust
    // Todo allow multiple tags
    // Todo allow onExit events
    // Todo allow onStay events
    // Todo allow to mark entities and not allow that specific one to invoke the events
    
    [SerializeField] protected string targetTag;
    [SerializeField] protected UnityEvent onEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            onEnter.Invoke();
        }
    }
}
