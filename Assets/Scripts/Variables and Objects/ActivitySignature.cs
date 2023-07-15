using System;
using System.Collections;
using System.Collections.Generic;
using I302.Manu;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Activity Signature", menuName = "Signatures/Activity Signature")]
public class ActivitySignature : SerializedScriptableObject
{
    // [FoldoutGroup("Activity Details"),PropertyOrder(70)] public ActionType actionType;
    [Title("Activity Settings")]
    [PropertyOrder(70)] public string ActionName;
    [PropertyOrder(70),PreviewField] public Sprite ActionIcon;

    [field: SerializeField,PropertyOrder(90),Space(10)] 
    public UnityEvent ActivityActions { get; private set; } = new();
    
    [field: SerializeField,PropertyOrder(80),Space(10)] 
    private List<Condition> Conditions { get; set; } = new();

    public bool IsConditionMet()
    {
        if (Conditions.Count == 0) return true;
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }

    public void InvokeActivity()
    {
        ActivityActions.Invoke();
        Conditions.ForEach(c => c.Use());
    }
}


