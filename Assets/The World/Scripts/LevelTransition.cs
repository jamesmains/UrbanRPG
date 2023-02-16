using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private PlayerSaveData saveData;

    [SerializeField] private bool debugRunOnStart = true;
    private void Start()
    {
        if (!saveData.Loaded && debugRunOnStart) // NOTE this should be for debugging only. It should be true before hitting this in build.
        {
            saveData.LoadLocation();
            if (saveData.NextLevelTransition == null)
            {
                saveData.Loaded = true;
                return;
            }
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(saveData.NextLevelTransition.TargetScene);
    }
}
