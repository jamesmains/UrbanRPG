using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameEventListener))]
public class Window : MonoBehaviour
{
#if UNITY_EDITOR
    public string descriptor;
#endif
    public GameObject windowObject;
    public bool isActive;

    private void Awake()
    {
        windowObject.SetActive(isActive = false);
    }

    public void ToggleWindow()
    {
        isActive = !isActive;
        windowObject.SetActive(isActive);
    }
}
