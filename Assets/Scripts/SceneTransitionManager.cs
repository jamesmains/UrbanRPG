using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private PlayerSaveSlot saveSlot;
    [SerializeField] private bool debugRunOnStart = true;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Start()
    {
        if (saveSlot == null) return;
        if (!saveSlot.Loaded && debugRunOnStart) // NOTE this should be for debugging only. It should be true before hitting this in build.
        {
            saveSlot.LoadData();
            if (saveSlot.NextSceneTransition == null)
            {
                saveSlot.Loaded = true;
                return;
            }
            BeginLoadingNextScene(saveSlot.NextSceneTransition);
        }
    }

    private void BeginLoadingNextScene(SceneTransition targetScene)
    {
        saveSlot.NextSceneTransition = targetScene;
        animator.SetTrigger("FadeOut");
    }

    public void LoadNextScene() // Called from animator
    {
        SceneManager.LoadScene(saveSlot.NextSceneTransition.TargetScene);
    }
}
