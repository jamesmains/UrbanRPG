using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ChanceEvent : MonoBehaviour
{
    [SerializeField] private float rarity;
    [SerializeField] private bool runOnAwake;
    [SerializeField] private UnityEvent onPass;
    [SerializeField] private UnityEvent onFail;
    
    private void Awake()
    {
        if (runOnAwake)
        {
            CheckChance();
        }
    }

    public void CheckChance()
    {
        float roll = Random.Range(0.0f, 100f);
        if (roll < rarity)
        {
            onPass.Invoke();
        }
        else onFail.Invoke();
    }
}
