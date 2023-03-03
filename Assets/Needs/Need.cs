using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "Player/Need")]
public class Need : ScriptableObject
{
    [FoldoutGroup("Details")] public string Name;
    [FoldoutGroup("Details")] public string Description;
    [FoldoutGroup("Details")] public Sprite Icon;
    [FoldoutGroup("Details")] public bool IsDepleted;
    [FoldoutGroup("Variables")] public float DecayRate;
    [FoldoutGroup("Variables")] [SerializeField] private float value;
    
    public float Value
    {
        get
        {
            this.value = Math.Clamp(this.value, 0, 100);
            IsDepleted = this.value <= 0;
            return this.value;
        }
        set
        {
            this.value = value;
            this.value = Math.Clamp(this.value, 0, 100);
            IsDepleted = this.value <= 0;
        }
    }
}
