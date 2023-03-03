using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ObjectToggle : MonoBehaviour
{
    [SerializeField] private bool checkOnAwake;
    [FoldoutGroup("Events")][SerializeField] private UnityEvent onMeetsRequirements;
    [FoldoutGroup("Events")][SerializeField] private UnityEvent onFailsRequirements;
    [field: SerializeField] private List<ActivitySignature> Conditions { get; set; } = new();

    private void Awake()
    {
        if (checkOnAwake)
        {
            CheckStatus();
        }
    }

    public void CheckStatus()
    {
        bool canDo = IsConditionMet();

        if (canDo)
        {
            onMeetsRequirements.Invoke();    
        }
        else
        {
            onFailsRequirements.Invoke();
        }
    }
    
    private bool IsConditionMet()
    {
        return Conditions.TrueForAll(c => c.IsConditionMet());
    }
}