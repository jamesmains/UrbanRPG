using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "Scriptable Objects/Need")]
public class Need : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public bool IsDepleted;
    public float DecayRate;

    [SerializeField]
    private float value;
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
