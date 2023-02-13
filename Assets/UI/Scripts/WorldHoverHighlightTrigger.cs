using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHoverHighlightTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRendererGameEvent onMouseHover;
    [SerializeField] private BoolVariable mouseOverUserInterface;
    [SerializeField] private SpriteRenderer render;
    
    private bool isActive;  
    private void OnMouseOver()
    {
        if (!mouseOverUserInterface.Value && !isActive)
        {
            isActive = true;
            onMouseHover.Raise(render);
        }
        else if (mouseOverUserInterface.Value && isActive)
        {
            onMouseHover.Raise(null);
        }
    }

    private void OnMouseExit()
    {
        Clear();
    }

    private void OnDisable()
    {
        Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
    private void Clear()
    {
        onMouseHover.Raise(null);
        isActive = false;
    }
    
}
