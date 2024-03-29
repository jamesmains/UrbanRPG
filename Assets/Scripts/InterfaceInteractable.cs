using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InterfaceInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool canInteractWith;
    public UnityEvent onInteract;

    private void OnEnable()
    {
        GameEvents.OnPrimaryMouseButtonDown += Interact;
    }

    private void OnDisable()
    {
        GameEvents.OnPrimaryMouseButtonDown -= Interact;
    }

    private void Interact()
    {
        if (!canInteractWith) return;
        onInteract.Invoke();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        canInteractWith = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canInteractWith = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
