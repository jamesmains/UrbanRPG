using System;
using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    public class TimeEvent : SerializedMonoBehaviour {
        [ShowInInspector] [OdinSerialize] public List<TimeBasedEffect> Effects { get; set; } = new();

        private void Start() {
            foreach (var effect in Effects) effect.Init();
        }

        private void OnEnable() {
            foreach (var effect in Effects) GameEvents.OnNewDay.AddListener(effect.Init);
            foreach (var effect in Effects) GameEvents.OnChangeTime.AddListener(effect.Tick);
        }

        private void OnDisable() {
            foreach (var effect in Effects) GameEvents.OnNewDay.AddListener(effect.Init);
            foreach (var effect in Effects) GameEvents.OnChangeTime.AddListener(effect.Tick);
        }
    }

    [Serializable]
    public abstract class TimeBasedEffect {
        [SerializeField] [FoldoutGroup("Details")]
        public string Description;

        [SerializeField] [FoldoutGroup("Details")]
        public TimeVariable TimeVariable;

        [SerializeField] [FoldoutGroup("Details")]
        public bool InvokeIfOver;

        [SerializeField] [FoldoutGroup("Details")]
        public float TargetTime;

        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly]
        protected float timer;

        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly]
        protected bool ticking;

        public abstract void Init();
        public abstract void Tick();
        public abstract void InvokeEffect();
    }

    public class TimeEffect : TimeBasedEffect {
        public override void Init() {
            if (TimeVariable.Value > TargetTime) // Passed the check point
            {
                if (InvokeIfOver)
                    InvokeEffect();
                return;
            }

            timer = TargetTime - TimeVariable.Value;
            ticking = true;
        }

        public override void Tick() {
            if (!ticking) return;
            if (timer > 0) timer -= Time.deltaTime * TimeManager.TimeMultiplier;
            if (!(timer <= 0)) return;
            InvokeEffect();
        }

        public override void InvokeEffect() {
            ticking = false;

            // DEBUG TIME OUTPUT
            // float hour = (int) Math.Truncate(TimeVariable.Value / 60f);
            // int minute = Mathf.FloorToInt((TimeVariable.Value % 60f));
            // string addendum = "";
            // addendum = hour > 11 ? "PM" : "AM";
            // if (hour > 12)
            // {
            //     hour -= 12;    
            // }
            // Debug.Log($"{hour:00}:{minute:00} {addendum}");
        }
    }

    public class ObjectToggleTimeEffect : TimeEffect {
        public bool SetState;
        public GameObject TargetObject;

        public override void InvokeEffect() {
            base.InvokeEffect();
            TargetObject.SetActive(SetState);
        }
    }

    public class InvokeEventTimeEffect : TimeEffect {
        public UnityEvent OnTimeMet;

        public override void InvokeEffect() {
            base.InvokeEffect();
            OnTimeMet.Invoke();
        }
    }
}