using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private PlayerSaveSlot saveSlot;
    [SerializeField] private bool debugRunOnStart = true;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        GameEvents.OnLoadNextScene += BeginLoadingNextScene;
    }

    private void OnDisable()
    {
        GameEvents.OnLoadNextScene -= BeginLoadingNextScene;
    }

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
            BeginLoadingNextScene();
        }
    }

    private void BeginLoadingNextScene()
    {
        animator.SetTrigger("FadeOut");
    }

    public void LoadNextScene() // Called from animator
    {
        SceneManager.LoadScene(saveSlot.NextLevelTransition.TargetScene);
    }
}
