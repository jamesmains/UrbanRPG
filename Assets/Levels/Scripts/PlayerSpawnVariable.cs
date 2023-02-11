using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Player Spawner Variable", menuName = "Variables/Player Spawner Variable")]
public class PlayerSpawnVariable : ScriptableObject
{
    public LevelTransitionSignature DefaultValue;
    public LevelTransitionSignature NextLevelTransition;
    
    private void OnEnable()
    {
        NextLevelTransition = DefaultValue;
    }
}
