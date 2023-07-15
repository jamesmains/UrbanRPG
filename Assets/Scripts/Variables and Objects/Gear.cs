using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Gear", menuName = "Unsorted/Gear")]
public class Gear : Item
{
    public GearType GearType;
    public string spriteSheetId;

#if UNITY_EDITOR
    [Button]
    public void SetTempName()
    {
        Name = spriteSheetId;
    }
    
    [Button]
    public void SetID()
    {
        string relativePath = Path.GetDirectoryName(Application.dataPath+"/"+AssetDatabase.GetAssetPath(this).Split("Assets/")[1]);
        var relativeDir = Directory.GetFiles(relativePath).ToList();
        var targetString = relativeDir.FindAll(o => o.Contains(".png") && !o.Contains(".meta"));
        spriteSheetId = targetString[0].Split("Resources\\")[1].Replace(".png","");
    }
#endif
    
}