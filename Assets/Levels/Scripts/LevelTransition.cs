using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private LevelTransitionSignature targetLevelTransitionSignature;
    [SerializeField] private PlayerSpawnVariable spawnVariable;

    private void OnTriggerEnter(Collider other)
    {
        spawnVariable.NextLevelTransition = targetLevelTransitionSignature;
        SceneManager.LoadScene(targetLevelTransitionSignature.TargetScene);
    }
}
