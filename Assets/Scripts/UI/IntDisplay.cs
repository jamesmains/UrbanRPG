using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntDisplay : MonoBehaviour
{
    [SerializeField] private string prefix, suffix; 
    [SerializeField] private IntVariable intVariable;
    [SerializeField] private TextMeshProUGUI textDisplay;

    public void Update()
    {
        textDisplay.text = $"{prefix}{intVariable.Value}{suffix}";
    }
}
