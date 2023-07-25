using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BannerDisplayPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        GameEvents.OnReputationChange += InvokeReputationChange;
    }

    private void OnDisable()
    {
        GameEvents.OnReputationChange -= InvokeReputationChange;
    }

    private void InvokeReputationChange(Actor actor)
    {
        text.text = $"Now <allcaps>{actor.GetCurrentReputationTier()}</allcaps> with <allcaps>{actor.actorName}</allcaps>!";
        animator.SetTrigger("Invoke");
    }
}
