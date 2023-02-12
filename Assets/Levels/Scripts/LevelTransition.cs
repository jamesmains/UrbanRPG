using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private PlayerSpawnVariable spawnVariable;

    private void Awake()
    {
        if (!spawnVariable.Loaded) // NOTE this should be for debugging only. It should be true before hitting this in build.
        {
            spawnVariable.LoadLocation();
            if (spawnVariable.NextLevelTransition == null)
            {
                spawnVariable.Loaded = true;
                return;
            }
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(spawnVariable.NextLevelTransition.TargetScene);
    }
}
