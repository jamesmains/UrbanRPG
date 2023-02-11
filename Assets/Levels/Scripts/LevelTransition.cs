using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private PlayerSpawnVariable spawnVariable;

    private void LoadNextScene()
    {
        SceneManager.LoadScene(spawnVariable.NextLevelTransition.TargetScene);
    }
}
