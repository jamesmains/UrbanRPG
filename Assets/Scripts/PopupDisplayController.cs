using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject HorizontalPopup;
    [SerializeField] private GameObject IconPopupObject;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void OnCreateHorizontalPopup(Sprite icon, string value)
    {
        Instantiate(HorizontalPopup, this.transform).GetComponent<DisplayPopup>().Setup(icon,value);
    }

    public void OnCreateIconPopup(Sprite icon)
    {
        Instantiate(IconPopupObject, this.transform).GetComponent<DisplayPopup>().Setup(icon);
    }
}
