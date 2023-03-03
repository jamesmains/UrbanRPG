using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monthNameDisplay;
    [SerializeField] private GameObject calendarDayDisplayObject;
    [SerializeField] private Transform calendaryDisplayContainer;
    
    [SerializeField] private TimeVariable dayVariable;
    [SerializeField] private TimeVariable monthVariable;
    [SerializeField] private Slider yearProgressDisplay;
    [SerializeField] private Sprite multipleEventsOnDaySprite;

    public TextMeshProUGUI calendarDayDetailsText;
    
    public List<CalendarSignature> calendarSignatures = new();
    
    private void Awake()
    {
        UpdateCalendar();
    }

    private void Update()
    {
        monthNameDisplay.text = $"{((Month) monthVariable.Value).ToString()} the {UtilFunctions.AddOrdinal((int)(dayVariable.Value+1))}";
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
            
            var display = obj.GetComponent<CalendarDayDisplay>();
            
            calendarDayDetailsText.text = "";
            display.Setup(isHighlighted,day,icon);
            display.AssignInteract(delegate { calendarDayDetailsText.text = detailString; display.ToggleTemporaryHighlight(true); });
            if (isHighlighted)
            {
                display.ToggleTemporaryHighlight(true);
                display.Interact();
            }
            // display.AssignMouseExit(delegate { calendarDayDetailsText.text = ""; });
        }
    }
    
#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        calendarSignatures.Clear();
        var quests = AssetDatabase.FindAssets("t:CalendarSignature");
        
        for (int i = 0; i < quests.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( quests[i] );
            var asset = AssetDatabase.LoadAssetAtPath<CalendarSignature>( assetPath );
            calendarSignatures.Add(asset);
        }
    }
#endif
}
