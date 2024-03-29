using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "Signatures/Need")]
public class Need : ScriptableObject
{
    [FoldoutGroup("Details")] public string Name;
    [FoldoutGroup("Details"), TextArea] public string Description;
    [FoldoutGroup("Details")] public Sprite Icon;
    [FoldoutGroup("Data")] public bool IsDepleted;
    [FoldoutGroup("Data")] public bool CanCausePassout;
    [FoldoutGroup("Data")] public float DecayRate;
    [FoldoutGroup("Data")] [SerializeField, Range(0,100)] private float value;
    
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
