using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntDisplay : MonoBehaviour
{
    [SerializeField] private IntVariable intVariable;
    [SerializeField] private TextMeshProUGUI textDisplay;

    private void Awake()
    {
        UpdateIntDisplay();
    }

    public void UpdateIntDisplay()
    {
        textDisplay.text = $"${intVariable.Value}";
    }
}
