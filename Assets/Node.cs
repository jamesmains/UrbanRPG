using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject NodeObject;
    [SerializeField] private float ReactivationTime;
    private float timer;
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime * TimeManager.TimeMultiplier;
            if (timer <= 0)
            {
                Activate();
            }
        }
    }

    public void Activate()
    {
        timer = -1;
        NodeObject.SetActive(true);   
    }

    public void Deactivate()
    {
        timer = ReactivationTime;
        NodeObject.SetActive(false);
    }
}
