using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monthNameDisplay;
    [SerializeField] private GameObject calendarDayDisplayObject;
    [SerializeField] private Transform calendaryDisplayContainer;
    
    [SerializeField] private TimeVariable dayVariable;
    [SerializeField] private TimeVariable monthVariable;
    [SerializeField] private Slider yearProgressDisplay; // todo do we need this?

    public TextMeshProUGUI thing;
    
    public List<CalendarSignature> calendarSignatures = new();
    
    private void Awake()
    {
        UpdateCalendar();
        // print(testSignature.IsConditionMet(0,0));
    }

    private void Update()
    {
        monthNameDisplay.text = ((Month) monthVariable.Value).ToString();
        if (yearProgressDisplay != null)
        {
            yearProgressDisplay.value = Mathf.InverseLerp(0, 5, monthVariable.Value);
        }
    }

    public void UpdateCalendar()
    {
        var oldItems = calendaryDisplayContainer.GetComponentsInChildren<Transform>();
        foreach (Transform item in oldItems) {
            if(item!=calendaryDisplayContainer)
                GameObject.Destroy(item.gameObject);
        }
        for (int day = 0; day < 28; day++)
        {
            var obj = Instantiate(calendarDayDisplayObject, calendaryDisplayContainer);
            bool isHighlighted = day == (int) dayVariable.Value;
            int dayOffset = day + 1;
            string displayText = "";
            Sprite icon = null;
            foreach (CalendarSignature signature in calendarSignatures)
            {
                if (signature.IsConditionMet(day, -1))
                {
                    displayText += $"<b>{signature.DisplayName}</b><br>";
                    displayText += $"{signature.DisplayText}<br>";
                    icon = signature.DisplayIcon;
                }
            }
            // Sprite icon = testSignature.IsConditionMet(i,-1) ? testSignature.DisplayIcon : null; // todo replace so its not null
            var display = obj.GetComponent<CalendarDayDisplay>();
            display.Setup(isHighlighted,dayOffset,icon);
            display.AssignMouseOver(delegate { thing.text = displayText; });
            display.AssignMouseExit(delegate { thing.text = ""; });
        }
    }
}
