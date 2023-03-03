using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectToggle : MonoBehaviour
{
    [field: SerializeField] private List<ActivitySignature> Conditions { get; set; } = new();
    [SerializeField] private bool checkOnAwake;
    [SerializeField] private UnityEvent onMeetsRequirements;
    [SerializeField] private UnityEvent onFailsRequirements;
    
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