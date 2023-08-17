using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Gear", menuName = "Items/Gear")]
public class Gear : Item
{
    [field: SerializeField,PropertyOrder(60),Space(10)]
    public GearType GearType;
    [field: SerializeField,PropertyOrder(80),Space(10)] 
    public List<GearEffect> GearEffects { get; set; } = new();
    public string spriteSheetId;

#if UNITY_EDITOR
    [Button]
    public void SetTempName()
    {
        Name = spriteSheetId.Split("\\").Last();
    }
    
    [Button]
    public void SetID()
    {
        string relativePath = Path.GetDirectoryName(Application.dataPath+"/"+AssetDatabase.GetAssetPath(this).Split("Assets/")[1]);
        var relativeDir = Directory.GetFiles(relativePath).ToList();
        var targetString = relativeDir.FindAll(o => o.Contains(".png") && !o.Contains(".meta"));
        if(targetString[0].Contains(".png"))
            targetString[0] = targetString[0].Split("Resources\\")[1].Replace(".png","");
        spriteSheetId = targetString[0];
    }
#endif
    
}

[Serializable]
public abstract class GearEffect
{
    public abstract void OnEquip();
    public abstract float GetEffectValue();
    public string effectText;
}

public class RideEffect : GearEffect
{
    [SerializeField] private float modValue = new();
    
    public override void OnEquip()
    {
        GameEvents.OnChangeRide.Raise();
    }

    public override float GetEffectValue()
    {
        return modValue;
    }
}

[Serializable]
public class GearOption
{
    public GearOption(Gear g, GearType gearType)
    {
        gear = g;
        type = gearType;
    }
    public Gear gear;
    public GearType type;
}
