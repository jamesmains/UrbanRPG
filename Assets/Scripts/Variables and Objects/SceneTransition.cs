using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level Transition", menuName = "Signatures/Level Transition")]
public class SceneTransition : ScriptableObject
{
    public String TargetScene;
    public Vector3 SpawnLocation;
}