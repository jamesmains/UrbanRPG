using System;
using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    public class ActivityTrigger : SerializedMonoBehaviour {
        public static bool ActivityLock;

        [SerializeField] [FoldoutGroup("Details")] [TextArea]
        private string description;

        [ReadOnly] public bool isActive;
        [OdinSerialize] public List<ActivityAction> Activities = new();

        private string identity;

        public string Identity {
            get {
                if (string.IsNullOrEmpty(identity)) identity = Guid.NewGuid().ToString();
                return identity;
            }
        }

        private void OnEnable() {
        }

        private void OnDisable() {
            GameEvents.OnCloseActivityWheel.Invoke();

            isActive = false;
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                GameEvents.OnCloseActivityWheel.Invoke();
                isActive = false;
            }
        }

        private void OnTriggerStay(Collider other) {
            if (isActive || ActivityLock || Global.PlayerLock > 0) return;
            if (other.CompareTag("Player")) {
                GameEvents.OnOpenActivityWheel.Invoke(this);
                isActive = true;
            }
        }
    }

    public class ActivityAction {
        public Activity signature;
        [FoldoutGroup("World Action")] public UnityEvent worldActions = new();
        [ShowInInspector] [OdinSerialize] public List<Condition> specialConditions { get; set; } = new();

        public void InvokeSpecialActions() {
            specialConditions.ForEach(c => c.Use());
        }

        public bool IsConditionMet() {
            if (specialConditions.Count == 0) return true;
            return specialConditions.TrueForAll(c => c.IsConditionMet());
        }

        public void AssignListeners(Action[] actions) {
            for (var i = 0; i < actions.Length; i++) {
                var actionIndex = i;
                worldActions.AddListener(delegate { actions[actionIndex].Invoke(); });
            }
        }

        public void AssignListener(Action action) {
            worldActions.AddListener(action.Invoke);
        }
    }
}