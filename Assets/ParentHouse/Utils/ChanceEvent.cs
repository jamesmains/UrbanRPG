using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace ParentHouse.Utils {
    public class ChanceEvent : MonoBehaviour {
        [FoldoutGroup("Data")] [SerializeField]
        private float rarity;

        [FoldoutGroup("Data")] [SerializeField]
        private bool runOnAwake;

        [FoldoutGroup("Events")] [SerializeField]
        private UnityEvent onPass;

        [FoldoutGroup("Events")] [SerializeField]
        private UnityEvent onFail;

        private void Awake() {
            if (runOnAwake) CheckChance();
        }

        public void CheckChance() {
            var roll = Random.Range(0.0f, 100f);
            if (roll < rarity)
                onPass.Invoke();
            else onFail.Invoke();
        }
    }
}