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
    [FoldoutGroup("Details")]public string DisplayName;
    [FoldoutGroup("Details")][TextArea] public string DisplayText;
    [FoldoutGroup("Details")]public Sprite DisplayIcon;

    [field: SerializeField] private List<CalendarCondition> Conditions { get; set; } = new List<CalendarCondition>();
    
    public bool IsConditionMet(int day, int month)
    {
        return Conditions.TrueForAll(c => c.IsConditionMet(day,month));
    }
}


