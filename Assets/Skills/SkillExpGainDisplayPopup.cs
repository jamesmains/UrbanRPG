using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillExpGainDisplayPopup : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI valueText;
    public void Setup(Sprite icon, int value)
    {
        iconImage.sprite = icon;
        valueText.text = $"+{value}";
    }

    private void OnFinish()
    {
        Destroy(this.gameObject);
    }
}
