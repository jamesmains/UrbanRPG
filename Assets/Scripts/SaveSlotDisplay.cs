using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveSlotDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterNameDisplay;
    [SerializeField] private TextMeshProUGUI cityNameDisplay;
    [SerializeField] private Button button;

    public void Setup(string characterName, string cityName, UnityAction actions)
    {
        characterNameDisplay.text = characterName;
        cityNameDisplay.text = cityName;
        button.onClick.AddListener(actions);
    }
}
