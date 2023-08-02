using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NeedsManager : MonoBehaviour
{
    [SerializeField] private Need[] playerNeeds;
    [SerializeField] private SceneTransition hospitalScene;
    [FoldoutGroup("Events")][SerializeField] private List<NeedsTriggerEvent> triggerEvents = new();

    private bool passedOut = false;
    
    private void PassOut()
    {
        if (passedOut) return;
        passedOut = true;
        GameEvents.OnSendGenericMessage.Raise("You passed out...");
        Global.PlayerLock++;
        foreach (var need in playerNeeds)
        {
            if (need.Value < 50)
                need.Value = 50;
        }
        Invoke(nameof(DelayPassout), 1);
    }
    
    void DelayPassout()
    {
        GameEvents.OnLoadNextScene.Raise(hospitalScene);
        GameEvents.OnPassout.Raise();
        passedOut = false;
    }
    
    void Update()
    {
        if (passedOut) return;
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
            if(playerNeeds[i].IsDepleted && playerNeeds[i].CanCausePassout)
                PassOut();
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
