using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Time", menuName = "Variables/Time")]
public class TimeVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public float Value;

#if UNITY_EDITOR
    public bool ShowChain;
#endif


    [ShowIf("ShowChain")]
    public TimeVariable ChainVariable;

    [ShowIf("ShowChain")]
    public float MaxValue;
    
    public void SetValue(float value)
    {
        Value = value;
        if (Value >= MaxValue && ChainVariable != null)
        {
            Value = 0;
            ChainVariable.ApplyChange(1);
        }
        GameEvents.OnChangeTime.Raise();
    }

    public void SetValue(FloatVariable value)
    {
        Value = value.Value;
        if (Value >= MaxValue && ChainVariable != null)
        {
            Value = 0;
            ChainVariable.ApplyChange(1);
        }
        GameEvents.OnChangeTime.Raise();
    }

    [Button]
    public void ApplyChange(float amount)
    {
        Value += amount;
        if (Value >= MaxValue && ChainVariable != null)
        {
            Value = 0;
            ChainVariable.ApplyChange(1);
        }
        GameEvents.OnChangeTime.Raise();
    }

    public void ApplyChange(FloatVariable amount)
    {
        Value += amount.Value;
        if (Value >= MaxValue && ChainVariable != null)
        {
            Value = 0;
            ChainVariable.ApplyChange(1);
        }
        GameEvents.OnChangeTime.Raise();
    }
}
