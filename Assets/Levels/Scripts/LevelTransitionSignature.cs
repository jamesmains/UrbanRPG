using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level Transition Signature", menuName = "Scriptable Objects/Level Transition Signature")]
public class LevelTransitionSignature : ScriptableObject
{
    public String TargetScene;
    public Vector3 SpawnLocation;
}