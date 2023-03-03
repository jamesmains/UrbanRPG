using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Activity Signature", menuName = "Signatures/Activity Signature")]
public class ActivitySignature : SerializedScriptableObject
{
    [FoldoutGroup("Details")] public ActionType actionType;
    [FoldoutGroup("Details")] public string ActionName;
    [FoldoutGroup("Details")] public Sprite ActionIcon;

    [field: SerializeField] private List<Condition> Conditions { get; set; } = new();
    
    public bool IsConditionMet()
    {
        if (Conditions.Count == 0) return true;
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }

    public void TryUseItems()
    {
        Conditions.ForEach(c => c.Use());
    }
}


