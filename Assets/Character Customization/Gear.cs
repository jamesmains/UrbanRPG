using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Gear", menuName = "Scriptable Objects/Gear")]
public class Gear : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public AnimationSheet[] GearAnimationSheets;
    // public List<CustoAnimation> animations = new List<CustoAnimation>();
}
