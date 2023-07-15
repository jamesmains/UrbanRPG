using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "Dialogue/Actor")]
public class Actor : ScriptableObject
{
    public string actorName;
    public Sprite actionIcon;
    public Color hairColor;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearBody;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearShoes;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearPants;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearMouth;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearEyes;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearShirt;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearHair;
    [FoldoutGroup("Gear"), SerializeField] public Gear GearAccessory;
}
