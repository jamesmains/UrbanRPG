using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : Window
{
    [FoldoutGroup("Display")][SerializeField] private TextMeshProUGUI monthNameDisplay;
    [FoldoutGroup("Display")][SerializeField] private Transform calendaryDisplayContainer;
    [FoldoutGroup("Display")][SerializeField] private GameObject calendarDayDisplayObject;
    [FoldoutGroup("Data")][SerializeField] private TimeVariable dayVariable;
    [FoldoutGroup("Data")][SerializeField] private TimeVariable monthVariable;
    [FoldoutGroup("Details")][SerializeField] private Sprite multipleEventsOnDaySprite;

    [SerializeField] private Window detailsWindow;
    public TextMeshProUGUI calendarDayDetailsText;
    public List<CalendarSignature> calendarSignatures = new();
    
    private readonly List<CalendarDayDisplay> calendarDaySlots = new();
    
    public override void Show()
    {
        base.Show();
        detailsWindow.Hide();
        UpdateCalendar();
        GameEvents.OpenCalendarDayDetails += detailsWindow.Show;
    }

    public override void Hide()
    {
        base.Hide();
        GameEvents.OpenCalendarDayDetails -= detailsWindow.Show;
        detailsWindow.Hide();
    }

    private void Update()
    {
        monthNameDisplay.text = $"{((Month) monthVariable.Value).ToString()} the {UtilFunctions.AddOrdinal((int)(dayVariable.Value+1))}";
    }

    public void UpdateCalendar()
    {
        for (int day = 0; day < 28; day++)
        {
            if (calendarDaySlots.Count - 1 < day || calendarDaySlots[day] == null)
            {
                var obj = Instantiate(calendarDayDisplayObject, calendaryDisplayContainer).GetComponent<CalendarDayDisplay>();
                calendarDaySlots.Add(obj);
            }
            
            bool isHighlighted = day == (int) dayVariable.Value;
            int signaturesPerDayCount = 0;
            string detailString = "";
            Sprite icon = null;

            detailString += $"<b><u>{((Month) monthVariable.Value).ToString()} the {UtilFunctions.AddOrdinal((int)(day+1))}</b></u><br><br>";
            foreach (CalendarSignature signature in calendarSignatures)
            {
                if (signature.Active == false) continue;
                if (signature.IsConditionMet(day, -1))
                {
                    detailString += $"<b>{signature.DisplayName}</b><br>";
                    detailString += $"{signature.DisplayText}<br>";
                    icon = signature.DisplayIcon;
                    signaturesPerDayCount++;
                }
            }
            
            if (signaturesPerDayCount > 1)
            {
                icon = multipleEventsOnDaySprite;
            }
            
            var display = calendarDaySlots[day];
            
            calendarDayDetailsText.text = "";
            display.Setup(isHighlighted,day,icon,this);
            display.AssignInteract(delegate
            {
                calendarDayDetailsText.text = detailString; 
                display.ToggleTemporaryHighlight(true);
            });
            if (isHighlighted)
            {
                display.ToggleTemporaryHighlight(true);
                display.Interact();
            }
        }
    }
    
#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        calendarSignatures.Clear();
        var assets = AssetDatabase.FindAssets("t:CalendarSignature");
        
        foreach (var t in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( t );
            var asset = AssetDatabase.LoadAssetAtPath<CalendarSignature>( assetPath );
            calendarSignatures.Add(asset);
        }
    }
#endif
}
