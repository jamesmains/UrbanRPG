using System;
using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class NeedsManager : MonoBehaviour
    {
        [SerializeField] private bool canPassOut = true; // Todo flag as true for build, For debugging only
        [SerializeField] private Need[] playerNeeds;
        [SerializeField] private SceneTransition hospitalScene;
        [FoldoutGroup("Events")][SerializeField] private List<NeedsTriggerEvent> triggerEvents = new();

        private bool passedOut = false;
    
        private void PassOut()
        {
            if (passedOut || !canPassOut) return;
            print("Passed out");
            passedOut = true;
            GameEvents.OnSendGenericMessage.Invoke("You passed out...");
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
            GameEvents.OnLoadNextScene.Invoke(hospitalScene);
            GameEvents.OnPassout.Invoke();
            passedOut = false;
        }
    
        void Update()
        {
            if (passedOut || Global.PlayerLock > 0) return;
            foreach (var need in playerNeeds)
            {
                need.Value -= UnityEngine.Time.deltaTime * need.DecayRate * TimeManager.TimeMultiplier;
                foreach (var e in triggerEvents)
                {
                    if (need != e.targetNeed) continue;
                    if (e.targetNeed.Value <= e.targetAmount && e.canTrigger)
                    {
                        e.canTrigger = false;
                        GameEvents.OnNeedDecayTrigger.Invoke(e.outSprite);
                    }
                    else if (e.targetNeed.Value > e.targetAmount)
                    {
                        e.canTrigger = true;
                    }
                }
                if(need.IsDepleted && need.CanCausePassout)
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
}