using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Gear", menuName = "Unsorted/Gear")]
public class Gear : ScriptableObject
{
    [FoldoutGroup("Details")] public string Name;
    [FoldoutGroup("Details")] public string Description;
    [FoldoutGroup("Details")] public Sprite Icon;
    public AnimationSheet[] GearAnimationSheets;
}
