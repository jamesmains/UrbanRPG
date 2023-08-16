using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GearCategory : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public void Setup(UnityAction buttonDelegate, Sprite categoryIcon)
    {
        button.onClick.AddListener(buttonDelegate);
        image.sprite = categoryIcon;
    }
}
