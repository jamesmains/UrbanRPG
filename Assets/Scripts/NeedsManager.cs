using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] private Need[] playerNeeds;
    [FoldoutGroup("Events")][SerializeField] private List<NeedsTriggerEvent> triggerEvents = new();
    
    void Update()
    {
        for (int i = 0; i < playerNeeds.Length; i++)
        {
            playerNeeds[i].Value -= Time.deltaTime * playerNeeds[i].DecayRate;
            foreach (var e in triggerEvents)
            {
                if (playerNeeds[i] != e.targetNeed) continue;
                if (e.targetNeed.Value <= e.targetAmount && e.canTrigger)
                {
                    e.canTrigger = false;
                    GameEvents.OnNeedDecayTrigger.Raise(e.outSprite);
                }
                else if (e.targetNeed.Value > e.targetAmount)
                {
                    e.canTrigger = true;
                }
            }
        }
    }
}

[Serializable]
public class NeedsTriggerEvent
{
    public float targetAmount;
    public Need targetNeed;
    public Sprite outSprite;
    public bool canTrigger = true;
}