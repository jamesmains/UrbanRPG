using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Gear Collection", menuName = "Collections/GearCollection")]
public class GearCollection : ScriptableObject
{
    public string header;
    public Sprite icon;
    public GearType type;
    public bool allowNull = false;
    public List<Gear> gear = new();
    
#if UNITY_EDITOR
    [Button]
    public void FindAssetsByType()
    {
        gear.Clear();
        var assets = AssetDatabase.FindAssets("t:Gear");
        foreach (var t in assets)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath( t );
            var asset = AssetDatabase.LoadAssetAtPath<Gear>( assetPath );
            if(asset.GearType == type)
                gear.Add(asset);
        }
    } 
#endif
}