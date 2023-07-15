using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverUserInterfaceUtil : MonoBehaviour
{
    [SerializeField] private LayerMask UILayer;
    private int layer;

    private void Awake()
    {
        layer = 5;
    }

    private void Update()
    {
        Global.IsMouseOverUI = IsPointerOverUIElement();
    }
 
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
 
 
    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == layer)
            {
                if(UrbanDebugger.DebugLevel>=1)
                {
                    Debug.Log($"Mouse is over {curRaysastResult.gameObject.name} (MouseOverUserInterfaceUtil.cs)");
                }
                return true;
            }
        }
        return false;
    }
 
 
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
}
