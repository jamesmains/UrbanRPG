using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] private Need[] playerNeeds;
    void Update()
    {
        for (int i = 0; i < playerNeeds.Length; i++)
        {
            playerNeeds[i].Value -= Time.deltaTime * playerNeeds[i].DecayRate;
        }
    }
}
