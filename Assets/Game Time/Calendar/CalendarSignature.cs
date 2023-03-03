using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Calendar Signature", menuName = "Signatures/Calendar Signature")]
public class CalendarSignature : SerializedScriptableObject
{
    public bool Active;
    public string DisplayName;
    [TextArea] public string DisplayText;
    public Sprite DisplayIcon;

    [field: SerializeField] private List<CalendarCondition> Conditions { get; set; } = new List<CalendarCondition>();
    
    public bool IsConditionMet(int day, int month)
    {
        return Conditions.TrueForAll(c => c.IsConditionMet(day,month));
    }
}


