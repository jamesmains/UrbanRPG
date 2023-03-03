using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Int", menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public int Value;
    public GameEvent OnValueChanged;
    
    public IntVariable ChainVariable;
    public int MaxValue;
    public void SetValue(int value)
    {
        Value = value;
    }

    public void SetValue(IntVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(int amount)
    {
        Value += amount;
        if (Value >= MaxValue && ChainVariable != null)
        {
            Value = 0;
            ChainVariable.ApplyChange(1);
        }
        OnValueChanged?.Raise();
    }

    public void ApplyChange(IntVariable amount)
    {
        Value += amount.Value;
        OnValueChanged?.Raise();
    }
}