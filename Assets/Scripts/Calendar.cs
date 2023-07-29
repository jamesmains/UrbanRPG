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
    
    public TextMeshProUGUI calendarDayDetailsText;
    public List<CalendarSignature> calendarSignatures = new();
    
    private readonly List<CalendarDayDisplay> calendarDaySlots = new();

    private void OnEnable()
    {
        GameEvents.OnNewDay += UpdateCalendar;
    }

    private void OnDisable()
    {
        GameEvents.OnNewDay -= UpdateCalendar;
    }

    public override void Show()
    {
        base.Show();
        UpdateCalendar();
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

            detailString += $"<b><u>{((Month) monthVariable.Value).ToString()} the {UtilFunctions.AddOrdinal((int)(day+1))}</b></u><br><br>";
            foreach (CalendarSignature signature in calendarSignatures)
            {
                if (signature.Active == false) continue;
                if (signature.IsConditionMet(day, -1))
                {
                    detailString += $"<b>{signature.DisplayName}</b>";
                    detailString += $"{signature.DisplayText}<br>";
                    signaturesPerDayCount++;
                }
            }
            
            var display = calendarDaySlots[day];
            
            calendarDayDetailsText.text = "";
            display.Setup(isHighlighted,day,signaturesPerDayCount > 0,this);
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
    }    [Button]
    private void ClearDisplays()
    {
        calendaryDisplayContainer.DestroyChildrenInEditor();
        calendarDaySlots.Clear();
    }
#endif
}
