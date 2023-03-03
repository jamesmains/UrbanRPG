using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private PlayerSaveSlot saveSlot;
    [SerializeField] private bool debugRunOnStart = true;
    
    private void Start()
    {
        if (saveSlot == null) return;
        if (!saveSlot.Loaded && debugRunOnStart) // NOTE this should be for debugging only. It should be true before hitting this in build.
        {
            saveSlot.LoadData();
            if (saveSlot.NextLevelTransition == null)
            {
                saveSlot.Loaded = true;
                return;
            }
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(saveSlot.NextLevelTransition.TargetScene);
    }
}
