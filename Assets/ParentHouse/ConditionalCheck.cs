using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ParentHouse {
    public class ConditionalCheck : SerializedMonoBehaviour {
        [SerializeField] private bool continousCheck;
        [SerializeField] private float tickRate;

        [FoldoutGroup("Events")] [SerializeField]
        private UnityEvent onMeetsRequirements;

        [FoldoutGroup("Events")] [SerializeField]
        private UnityEvent onFailsRequirements;

        private bool cachedState;
        public List<Condition> Conditions = new();
        private float timer;

        private void Awake() {
            CheckStatus(true);
        }

        private void Update() {
            if (!continousCheck) return;
            if (timer <= 0) {
                CheckStatus();
                timer = tickRate;
            }
            else {
                timer -= Time.deltaTime;
            }
        }

        public void CheckStatus(bool resetCache = false) {
            var canDo = IsConditionMet();
            if (resetCache) cachedState = !canDo;

            if (canDo && !cachedState)
                onMeetsRequirements.Invoke();
            else if (!canDo && cachedState) onFailsRequirements.Invoke();
            cachedState = canDo;
        }

        private bool IsConditionMet() {
            return Conditions.TrueForAll(c => c.IsConditionMet());
        }
    }
}